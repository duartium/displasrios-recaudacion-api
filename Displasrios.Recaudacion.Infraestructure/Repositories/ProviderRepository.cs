using Displasrios.Recaudacion.Core.Contracts.Repositories;
using Displasrios.Recaudacion.Core.DTOs;
using Displasrios.Recaudacion.Core.Models;
using Displasrios.Recaudacion.Infraestructure.MainContext;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Displasrios.Recaudacion.Infraestructure.Repositories
{
    public class ProviderRepository : IProviderRepository
    {
        private readonly DISPLASRIOSContext _context;

        public ProviderRepository(DISPLASRIOSContext context)
        {
            _context = context;
        }

        public bool Create(ProviderCreate provider)
        {
            var _provider = new Proveedores
            {
                Nombre  = provider.Name,
                Direccion = provider.Address,
                Email = provider.Email,
                Ruc = provider.Ruc,
                Telefono = provider.Phone,
                Estado = true,
                CreadoEn = DateTime.Now,
                UsuarioCrea = provider.UserCreation
            };
            _context.Proveedores.Add(_provider);
            int resp = _context.SaveChanges();

            return resp > 0;
        }

        public IEnumerable<ProviderDto> GetAll()
        {
            return _context.Proveedores.Where(x => x.Estado)
                .Select(x => new ProviderDto { 
                    Id = x.IdProveedor,
                    Name = x.Nombre,
                    Address = x.Direccion,
                    Email = x.Email,
                    Phone = x.Telefono,
                    Ruc = x.Ruc,
                    UserCreation = x.UsuarioCrea
                }).ToArray();
        }

        public IEnumerable<ItemCatalogueDto> GetAsCatalogue()
        {
            return _context.Proveedores.Where(x => x.Estado)
                .Select(x => new ItemCatalogueDto
                {
                    Id = x.IdProveedor,
                    Description = x.Nombre
                }).ToArray();
        }

        public ProviderDto GetById(int id)
        {
            throw new NotImplementedException();
        }

        public ProviderDto GetByName(string name)
        {
            throw new NotImplementedException();
        }

        public bool Remove(int id)
        {
            var provider = _context.Proveedores.Where(x => x.Estado && x.IdProveedor == id).First();
            provider.Estado = false;
            _context.Update(provider);
            int rowsAffected =_context.SaveChanges();
            return rowsAffected > 0;
        }
    }
}
