using Displasrios.Recaudacion.Core.Contracts.Repositories;
using Displasrios.Recaudacion.Core.DTOs;
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
    }
}
