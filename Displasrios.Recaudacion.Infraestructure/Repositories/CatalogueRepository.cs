using Displasrios.Recaudacion.Core.Contracts.Repositories;
using Displasrios.Recaudacion.Core.DTOs;
using Displasrios.Recaudacion.Infraestructure.MainContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Displasrios.Recaudacion.Infraestructure.Repositories
{
    public class CatalogueRepository : ICatalogueRepository
    {
        private readonly DISPLASRIOSContext _context;

        public CatalogueRepository(DISPLASRIOSContext context)
        {
            _context = context;
        }

        public CatalogueDto Get(string name)
        {
            return (from cat in _context.Catalogos
                    join det in _context.ItemCatalogo on cat.IdCatalogo equals det.CatalogoId
                    where cat.Estado && det.Estado && cat.Nombre.Equals(name)
                    select new CatalogueDto
                    {
                        Id = cat.IdCatalogo,
                        Name = cat.Nombre,
                        Catalogue = cat.ItemCatalogo.Select(x => new ItemCatalogueDto
                        {
                            Id = x.IdItemCatalogo,
                            Description = x.Descripcion
                        }).ToList()
                    }).FirstOrDefault();
        }

        public List<CatalogueDto> GetAll()
        {
            return (from cat in _context.Catalogos
                    join det in _context.ItemCatalogo on cat.IdCatalogo equals det.CatalogoId
                    where cat.Estado && det.Estado
                    select new CatalogueDto { 
                        Id = cat.IdCatalogo,
                        Name = cat.Nombre,
                        Catalogue = cat.ItemCatalogo.Select(x => new ItemCatalogueDto { 
                            Id = x.IdItemCatalogo,
                            Description = x.Descripcion
                        }).ToList()
                    }).Distinct().ToList();
        }
    }
}
