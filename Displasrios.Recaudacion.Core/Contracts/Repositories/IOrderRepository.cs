using Displasrios.Recaudacion.Core.DTOs;
using Displasrios.Recaudacion.Core.DTOs.Sales;
using Displasrios.Recaudacion.Core.Models;
using System.Collections.Generic;

namespace Displasrios.Recaudacion.Core.Contracts.Repositories
{
    public interface IOrderRepository
    {
        IEnumerable<OrderSummaryDto> GetOrdersReceivable(FiltersOrdersReceivable filters);
        OrderReceivableDto GetOrderReceivable(int idOrder);
        bool RegisterPayment(OrderReceivableCreateRequest order, out string mensaje);
        IEnumerable<SummaryOrdersOfDay> GetSummaryOrdersOfDay();
        bool RecordVisit(VisitCreation visitCreation);
        bool CancelOrder(int idOrder, string username);
        decimal GetTotalSalesTodayBySeller(int idUser);
        SellerPersonalReportDto GetSellerPersonalReport(int idUser);
        SummaryOrdersOfDay GetSummaryOrder(int idOrder);
        IEnumerable<SummaryOrdersOfDay> GetSummaryOrdersByCustomer(int idCustomer);
        IEnumerable<SummaryOrdersOfDay> GetCollectionOfDay();
        CustomerDebtDto GetSellerDebts(int id);
    }
}