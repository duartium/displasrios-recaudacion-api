using Displasrios.Recaudacion.Core.DTOs;
using System.Collections.Generic;

namespace Displasrios.Recaudacion.Core.Contracts.Repositories
{
    public interface IProductRepository
    {
        IEnumerable<ProductDto> GetAll();
        ProductDto GetById(int id);
        IEnumerable<ProductSaleDto> GetForSale(string name);
        bool Create(ProductCreation product);
    }
}