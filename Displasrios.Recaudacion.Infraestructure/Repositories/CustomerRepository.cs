using Displasrios.Recaudacion.Core.Contracts.Repositories;
using Displasrios.Recaudacion.Core.DTOs;
using Displasrios.Recaudacion.Infraestructure.MainContext;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

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
                }).ToList();
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
    }
}
