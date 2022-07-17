using Displasrios.Recaudacion.Core.Contracts.Repositories;
using Displasrios.Recaudacion.Core.DTOs;
using Displasrios.Recaudacion.Core.Enums;
using Displasrios.Recaudacion.Core.Models;
using Displasrios.Recaudacion.Infraestructure.MainContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
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
    }
}
