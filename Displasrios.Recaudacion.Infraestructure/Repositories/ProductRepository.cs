using Displasrios.Recaudacion.Core.Contracts.Repositories;
using Displasrios.Recaudacion.Core.DTOs;
using Displasrios.Recaudacion.Infraestructure.MainContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Displasrios.Recaudacion.Infraestructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly DISPLASRIOSContext _context;
        public ProductRepository(DISPLASRIOSContext context)
        {
            _context = context;
        }
        public IEnumerable<ProductDto> GetAll()
        {
            return _context.Productos.Where(x => x.Estado)
                .Include(x => x.ProveedorId)
                .Select(x => new ProductDto
                {
                    Id = x.IdProducto,
                    Code = x.Codigo,
                    Name = x.Nombre,
                    Description = x.Descripcion,
                    Cost = x.Costo.ToString(),
                    SalePrice = x.PrecioVenta,
                    Stock = x.Stock,
                    QuantityPackage = (int)x.CantXPaquete,
                    QuantityLump = (int)x.CantXBulto,
                    Discount = x.Descuento.ToString(),
                    IvaTariff = (int)x.TarifaIva,
                    CategoryId = (int)x.CategoriaId,
                    ProdiverId = x.ProveedorId,
                    ProdiverName = x.Proveedor.Nombre
                }).ToList();
        }

        public ProductDto GetById(string id)
        {
            throw new NotImplementedException();
        }
    }
}
