using Displasrios.Recaudacion.Core.Contracts.Repositories;
using Displasrios.Recaudacion.Core.DTOs;
using Displasrios.Recaudacion.Core.Models;
using Displasrios.Recaudacion.Infraestructure.MainContext;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Displasrios.Recaudacion.Infraestructure.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly DISPLASRIOSContext _context;
        public OrderRepository(DISPLASRIOSContext context)
        {
            _context = context;
        }

        public IEnumerable<OrderSummaryDto> GetOrdersReceivable(FiltersOrdersReceivable filters)
        {
            return _context.Factura.Where(x => x.Estado == 1 && x.Etapa == 1 && x.Secuencial == null)
                .Include(client => client.Cliente)
                .Select(order => new OrderSummaryDto { 
                    Id = order.IdFactura,
                    OrderNumber = order.NumeroPedido.ToString().PadLeft(5, '0'),
                    FullNames = order.Cliente.Nombres + " " + order.Cliente.Apellidos,
                    TotalAmount = order.Total.ToString(),
                    Date = order.FechaEmision.ToString("dd/MM/yyyy")
                }).ToArray();
        }
    }
}
