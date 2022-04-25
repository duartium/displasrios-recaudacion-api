using Displasrios.Recaudacion.Core.Contracts.Repositories;
using Displasrios.Recaudacion.Core.DTOs;
using Displasrios.Recaudacion.Core.Models;
using Displasrios.Recaudacion.Infraestructure.MainContext;
using Microsoft.EntityFrameworkCore;
using System;
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

        public OrderReceivableDto GetOrderReceivable(int idOrder)
        {
            var orderReceivable = _context.Factura.Where(x => x.Estado == 1 && x.Etapa == 1)
                .Include(detail => detail.FacturaDetalle).ThenInclude(product => product.Producto)
                .Include(paymenths => paymenths.Pagos)
                .Include(client => client.Cliente)
                .Select(order => new OrderReceivableDto { 
                    Date = order.FechaEmision.ToString("dd/MM/yyyy"),
                    OrderNumber = order.NumeroPedido.ToString().PadLeft(5, '0'),
                    DaysDebt = (DateTime.Now - order.FechaEmision).Days,
                    TotalAmount = order.Total,
                    FullNames = order.Cliente.Nombres + " " + order.Cliente.Apellidos,
                    Payments = order.Pagos.Select(x => new PaymentDto { 
                        Amount = x.Cambio,
                        Date = x.Fecha.ToString("dd/mm/yyyy")
                        }).ToArray(),
                    Products = order.FacturaDetalle.Select(det => new ProductResumeDto { 
                        Name = det.Producto.Nombre,
                        Price = det.PrecioUnitario,
                        Quantity = det.Cantidad,
                        Total = Math.Round(det.Cantidad * det.PrecioUnitario, 2)
                    }).ToArray()
                }).FirstOrDefault();

            orderReceivable.Balance = orderReceivable.Payments.Length > 0
                ? orderReceivable.TotalAmount - orderReceivable.Payments.Sum(x => x.Amount) : orderReceivable.TotalAmount; 

            return orderReceivable;
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
