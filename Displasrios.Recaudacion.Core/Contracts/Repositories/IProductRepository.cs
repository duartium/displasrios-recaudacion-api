using Displasrios.Recaudacion.Core.DTOs;
using Displasrios.Recaudacion.Core.DTOs.Reports;
using System.Collections.Generic;

namespace Displasrios.Recaudacion.Core.Contracts.Repositories
{
    public interface IProductRepository
    {
        IEnumerable<ProductDto> GetAll();
        ProductDto GetById(int id);
        IEnumerable<ProductSaleDto> GetForSale(string name);
        bool Create(ProductCreation product);
        bool Remove(int id);
        IEnumerable<MostSelledProductDto> GetMostSelledProducts();
        int GetCurrentStock(int id);
        bool IncreaseStock(int id, int quantity);
        bool DecreaseStock(int id, int quantity);
    }
}