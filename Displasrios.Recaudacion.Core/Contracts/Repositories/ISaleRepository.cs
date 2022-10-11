using Displasrios.Recaudacion.Core.DTOs;
using Displasrios.Recaudacion.Core.DTOs.Sales;
using Displasrios.Recaudacion.Core.Models.Sales;
using System.Collections.Generic;

namespace Displasrios.Recaudacion.Core.Contracts.Repositories
{
    public interface ISaleRepository
    {
        SaleCreated Create(FullOrderDto order);
        IEnumerable<IncomeBySellersDto> GetIncomePerSellers(IncomeBySellers incomeBySellers);
        bool SaveCollectorSale(SalesSellerToday salesSellerToday);
        string GetSaleTemplateForEmail(int idInvoice);
        string GetEmailFromInvoice(int idInvoice);
    }
}
