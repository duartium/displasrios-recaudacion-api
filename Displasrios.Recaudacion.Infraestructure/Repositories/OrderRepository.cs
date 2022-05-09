﻿using Displasrios.Recaudacion.Core.Contracts.Repositories;
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
            var orderReceivable = _context.Factura.Where(x => x.Estado == 1 && x.Etapa == 1 && x.NumeroPedido == idOrder)
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
                        Amount = x.Pago,
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

        public bool RegisterPayment(OrderReceivableCreateRequest order, out string mensaje)
        {
            mensaje = $"Se ha registrado el abono de {order.CustomerPayment}.";
            using (var trans = _context.Database.BeginTransaction())
            {
                //registra el nuevo pago
                var pago = new Pagos
                {
                    Fecha = DateTime.Now,
                    FacturaId = order.IdOrder,
                    Pago = order.CustomerPayment,
                    Cambio = order.Change
                };
                _context.Pagos.Add(pago);
                _context.SaveChanges();

                var totals = _context.Factura.Where(x => x.Estado == 1 && x.IdFactura == order.IdOrder)
                    .Include(order => order.Pagos)
                    .Select(order => new PaymentsTotal { 
                        TotalOrder = order.Total,
                        TotalPaymentsCurrent = order.Pagos.Select(x => x.Pago).Sum()
                    }).First();

                int nuevoSecuencial = 0;
                if (totals.TotalPaymentsCurrent >= totals.TotalOrder)//Pedido pagado a totalidad: genera factura
                {
                    nuevoSecuencial = (int)_context.Secuenciales.Where(x => x.Nombre.Equals("factura"))
                                 .Select(x => x.Secuencial).FirstOrDefault() + 1;

                    //establece el nuevo secuencial
                    var secuencialRow = _context.Secuenciales.First(x => x.Nombre.Equals("factura"));
                    secuencialRow.Secuencial = nuevoSecuencial;
                    _context.Secuenciales.Update(secuencialRow);
                    _context.SaveChanges();

                    var invoice = _context.Factura.Where(x => x.Estado == 1 && x.IdFactura == order.IdOrder).First();
                    invoice.Secuencial = nuevoSecuencial;
                    _context.Factura.Update(invoice);
                    _context.SaveChanges();

                    mensaje = $"Se ha generado la factura Nº{nuevoSecuencial.ToString().PadLeft(5, '0')}";
                }
                
                _context.Database.CommitTransaction();
            }

            return true;
        }

        public IEnumerable<OrderSummaryDto> GetOrdersReceivable(FiltersOrdersReceivable filters)
        {
            return _context.Factura.Where(x => x.Estado == 1 && x.Etapa == 1 && x.Secuencial == null)
                .Include(client => client.Cliente)
                .Include(pagos => pagos.Pagos)
                .Select(order => new OrderSummaryDto { 
                    Id = order.IdFactura,
                    OrderNumber = order.NumeroPedido.ToString().PadLeft(5, '0'),
                    FullNames = order.Cliente.Nombres + " " + order.Cliente.Apellidos,
                    TotalAmount = order.Pagos.Count > 0 
                        ? Math.Round(order.Total - order.Pagos.Sum(x => x.Pago), 2).ToString() : order.Total.ToString(),
                    Date = order.FechaEmision.ToString("dd/MM/yyyy")
                }).ToArray();
        }

        public IEnumerable<SummaryOrdersOfDay> GetSummaryOrdersOfDay()
        {
            return _context.Factura.Where(x => x.Estado == 1)
                .Select(x => new SummaryOrdersOfDay {
                    IdOrder = x.IdFactura,
                    Date = x.CreadoEn.ToString("dd-MM-yyyy"),
                    OrderNumber = x.NumeroPedido.ToString().PadLeft(5, '0'),
                    Stage = x.Etapa,
                    TotalAmount = x.Total
                }).ToList();
        }
    }
}
