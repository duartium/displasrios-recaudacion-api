using Displasrios.Recaudacion.Core.Contracts.Repositories;
using Displasrios.Recaudacion.Core.DTOs;
using Displasrios.Recaudacion.Core.DTOs.Reports;
using Displasrios.Recaudacion.Infraestructure.MainContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

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
            var categories = _context.ItemCatalogo.Where(x => x.Estado && x.CatalogoId == 1005).ToArray();

            var products = _context.Productos.Where(x => x.Estado)
                .Include(x => x.Proveedor)
                .Select(x => new ProductDto
                {
                    Id = x.IdProducto,
                    Code = x.Codigo,
                    Name = x.Nombre,
                    Description = x.Descripcion,
                    Cost = x.Costo.ToString(),
                    SalePrice = x.PrecioVenta.ToString(),
                    Stock = x.Stock,
                    QuantityPackage = (int)x.CantXPaquete,
                    QuantityLump = (int)x.CantXBulto,
                    Discount = x.Descuento.ToString(),
                    IvaTariff = (int)x.TarifaIva,
                    CategoryId = (int)x.CategoriaId,
                    
                    ProdiverId = x.ProveedorId,
                    ProdiverName = x.Proveedor.Nombre
                }).OrderBy(x => x.Name).ToList();

            products.ForEach(x => x.CategorName = Array.Find(categories, z => z.IdItemCatalogo.Equals(x.CategoryId)).Descripcion);
            return products;
        }

        public IEnumerable<ProductSaleDto> GetForSale(string name)
        {
            return _context.Productos.Where(x => x.Estado && EF.Functions.Like(x.Nombre, $"%{name}%"))
                .Select(x => new ProductSaleDto
                {
                    Id = x.IdProducto,
                    Code = x.Codigo,
                    Name = x.Nombre,
                    SalePrice = x.PrecioVenta.ToString(),
                    Stock = x.Stock
                }).ToList();
        }

        public ProductDto GetById(int id)
        {
            return _context.Productos.Where(x => x.Estado && x.IdProducto == id)
               .Select(x => new ProductDto
               {
                   Id = x.IdProducto,
                   Code = x.Codigo,
                   Name = x.Nombre,
                   Description = x.Descripcion,
                   Cost = x.Costo.ToString(),
                   SalePrice = x.PrecioVenta.ToString(),
                   Stock = x.Stock,
                   QuantityPackage = (int)x.CantXPaquete,
                   QuantityLump = (int)x.CantXBulto,
                   Discount = x.Descuento.ToString(),
                   IvaTariff = (int)x.TarifaIva,
                   CategoryId = (int)x.CategoriaId,
                   ProdiverId = x.ProveedorId,
                   ProdiverName = x.Proveedor.Nombre
               }).FirstOrDefault();
        }

        public bool Create(ProductCreation productCreation)
        {
            var product = new Productos
            {
                Codigo = productCreation.Code,
                Nombre = productCreation.Name,
                Descripcion = productCreation.Description,
                Stock = productCreation.Stock,
                CategoriaId = productCreation.CategoryId,
                Costo = Decimal.Parse(productCreation.Cost, CultureInfo.InvariantCulture),
                PrecioVenta = Decimal.Parse(productCreation.SalePrice, CultureInfo.InvariantCulture),
                Descuento = 0,
                CantXPaquete = productCreation.QuantityPackage,
                CantXBulto = productCreation.QuantityLump,
                ProveedorId = productCreation.ProviderId,
                TarifaIva = productCreation.IvaTariff,
                UsuarioCrea = productCreation.UserCreation,
                Estado = true,
                CreadoEn = DateTime.Now
            };
            _context.Productos.Add(product);
            int rowsAffected = _context.SaveChanges();
            return rowsAffected > 0;
        }

        public bool Remove(int id)
        {
            var product = _context.Productos.Where(x => x.IdProducto == id && x.Estado).First();
            product.Estado = false;
            _context.Update(product);
            int rowAffected = _context.SaveChanges();
            return rowAffected > 0;
        }

        public IEnumerable<MostSelledProductDto> GetMostSelledProducts()
        {
            var products = _context.MostSelledProduct.FromSqlRaw("EXECUTE dbo.GET_MOST_SELLED_PRODUCTS").ToList();
            return products;
        }

        public int GetCurrentStock(int id)
        {
            return _context.Productos.Where(x => x.Estado && x.IdProducto == id)
                .Select(x => x.Stock).FirstOrDefault();
        }

        public bool IncreaseStock(int id, int quantity)
        {
            var product = _context.Productos.Where(x => x.Estado && x.IdProducto == id).FirstOrDefault();
            product.Stock += quantity;
            _context.Update(product);
            int resp = _context.SaveChanges();
            return resp > 0;
        }

        public bool DecreaseStock(int id, int quantity)
        {
            var product = _context.Productos.Where(x => x.Estado && x.IdProducto == id).FirstOrDefault();
            product.Stock -= quantity;
            _context.Update(product);
            int resp = _context.SaveChanges();
            return resp > 0;
        }
    }
}
