using Displasrios.Recaudacion.Core.Contracts.Repositories;
using Displasrios.Recaudacion.Core.DTOs;
using Displasrios.Recaudacion.Core.DTOs.Reports;
using Displasrios.Recaudacion.Core.Enums;
using Displasrios.Recaudacion.Core.Models;
using Displasrios.Recaudacion.Infraestructure.MainContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Displasrios.Recaudacion.Infraestructure.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly DISPLASRIOSContext _context;
        public CustomerRepository(DISPLASRIOSContext context)
        {
            _context = context;
        }

        public CustomerDto Get(int id)
        {
            return _context.Clientes.Where(x => x.Estado == true && x.IdCliente == id)
                .Select(x => new CustomerDto
                {
                    Id = x.IdCliente,
                    Identification = x.Identificacion,
                    IdentType = x.TipoIdentificacion,
                    Type = x.TipoCliente,
                    Names = x.Nombres,
                    Surnames = x.Apellidos,
                    Address = x.Direccion,
                    Email = x.Email,
                    PhoneNumber = x.Telefono,
                    CreatedAt = x.CreadoEn.ToString("dd-MM-yyyy")
                }).FirstOrDefault();
        }

        public IEnumerable<CustomerDto> GetAll()
        {
            return _context.Clientes.Where(x => x.Estado == true)
                .Select(x => new CustomerDto
                {
                    Id = x.IdCliente,
                    Identification = x.Identificacion,
                    IdentType = x.TipoIdentificacion,
                    Type = x.TipoCliente,
                    Names = x.Nombres,
                    Surnames = x.Apellidos,
                    Address = x.Direccion,
                    Email = x.Email,
                    PhoneNumber = x.Telefono,
                    CreatedAt = x.CreadoEn.ToString("dd-MM-yyyy")
                }).OrderBy(x => x.Names).ToList();
        }

        public CustomerSearchOrderDto GetByIdentification(string identification)
        {
            return _context.Clientes.Where(x => x.Estado && x.Identificacion.Equals(identification.Trim()))
                .Select(x => new CustomerSearchOrderDto { 
                    Id = x.IdCliente,
                    FullNames = $"{x.Nombres} {x.Apellidos}",
                    Identification = x.Identificacion
                }).FirstOrDefault();
        }

        public CustomerDebtDto GetDebts(string identification, string names)
        {
            IQueryable<Factura> query = null;
            var debts = new CustomerDebtDto();

            //identificación
            if (!String.IsNullOrEmpty(identification))
            {
                query = _context.Factura.Where(x => x.Estado == 1 && x.Etapa == 1 && x.Secuencial == null
                   && x.Cliente.Identificacion == identification);
            }
            else
            { //nombres
                query = _context.Factura.Where(x => x.Estado == 1 && x.Etapa == 1 && x.Secuencial == null
                    && x.Cliente.Nombres.Contains(names) || x.Cliente.Apellidos.Contains(names));
            }

            debts.OrdersReceivable = query.Include(client => client.Cliente).Include(pagos => pagos.Pagos)
                .Include(fac => fac.Usuario)
               .Select(order => new OrderSummaryDto
               {
                   Id = order.IdFactura,
                   OrderNumber = order.NumeroPedido.ToString().PadLeft(5, '0'),
                   FullNames = order.Cliente.Nombres + " " + order.Cliente.Apellidos,
                   Collector = order.Usuario.Usuario,
                   TotalAmount = order.Pagos.Count > 0
                       ? Math.Round(order.Total - order.Pagos.Sum(x => x.PagoReal), 2).ToString() : order.Total.ToString(),
                   Date = order.FechaEmision.ToString("dd/MM/yyyy")
               }).ToList();

            if (debts.OrdersReceivable != null && debts.OrdersReceivable.Count > 0)
            {
                debts.Fullnames = debts.OrdersReceivable.FirstOrDefault().FullNames.ToUpper();
                debts.TotalDebts = debts.OrdersReceivable.Sum(x => decimal.Parse(x.TotalAmount, CultureInfo.InvariantCulture));
            }

            return debts;
        }

        public CustomerSearchOrderDto GetByNames(string names)
        {
            return _context.Clientes.Where(x => x.Estado && x.Identificacion.Equals(names))
                .Select(x => new CustomerSearchOrderDto
                {
                    Id = x.IdCliente,
                    FullNames = $"{x.Nombres} {x.Apellidos}",
                    Identification = x.Identificacion
                }).FirstOrDefault();
        }

        CustomerSearchOrderDto[] ICustomerRepository.GetByNames(string names)
        {
            return _context.Clientes.Where(x => x.Estado && (EF.Functions.Like(x.Nombres, $"%{names}%") 
                || EF.Functions.Like(x.Apellidos, $"%{names}%")))
                .Select(x => new CustomerSearchOrderDto {
                    Id = x.IdCliente,
                    FullNames = $"{x.Nombres} {x.Apellidos}",
                    Identification = x.Identificacion
                }).ToArray();
        }

        public bool Update(CustomerUpdate customer)
        {
            int rowAffected = 0;

            using (var db = _context.Database.BeginTransaction())
            {
                var _customer = _context.Clientes.Where(x => x.IdCliente == customer.Id).FirstOrDefault();

                _customer.Identificacion = customer.Identification;
                _customer.Nombres = customer.Names;
                _customer.Apellidos = customer.Surnames;
                _customer.Direccion = customer.Address;
                _customer.Email = customer.Email;
                _customer.Telefono = customer.Phone;
                _customer.ModificadoEn = DateTime.Now;
                _customer.UsuarioMod = "default";
                
                _context.Clientes.Update(_customer);
                _context.Entry(_customer).State = EntityState.Modified;

                rowAffected = _context.SaveChanges();

                _context.Database.CommitTransaction();
            }

            return (rowAffected > 0);
        }

        public int Create(CustomerCreate customer)
        {
            var _customer = new Clientes
            {
                Identificacion = customer.Identification,
                TipoIdentificacion = TipoIdentificacion.C.ToString(),
                Nombres = customer.Names,
                Apellidos = customer.Surnames,
                Direccion = customer.Address,
                Email = customer.Email,
                Telefono = customer.Phone,
                TipoCliente = 1,
                Estado = true,
                CreadoEn = DateTime.Now,
                UsuarioCrea = customer.CurrentUser
            };
            _context.Clientes.Add(_customer);
            _context.SaveChanges();

            return _customer.IdCliente;
        }

        public bool Delete(int id)
        {
            var customer = _context.Clientes.Where(x => x.Estado && x.IdCliente == id).First();
            customer.Estado = false;
            _context.Update(customer).State = EntityState.Modified;
            int resp = _context.SaveChanges();
            return resp > 0;
        }

        public IEnumerable<BestCustomerDto> GetBestCustomers()
        {
            var customers = _context.BestCustomersResume.FromSqlRaw("EXECUTE dbo.GET_BEST_CUSTOMERS")
                .ToList().OrderByDescending(x => x.Totales);

            var result = new List<BestCustomerDto>();
            foreach (var item in customers)
            {
                var _customer = _context.Clientes.Where(x => x.Estado && x.IdCliente == item.IdCliente).First();
                result.Add(new BestCustomerDto
                {
                    Id = _customer.IdCliente,
                    FullNames = _customer.Nombres.ToUpper() + " " + _customer.Apellidos.ToUpper(),
                    TotalPurchases = item.Totales
                }); ;
            }

            return result;
        }
    }
}
