using Displasrios.Recaudacion.Core.Contracts.Repositories;
using Displasrios.Recaudacion.Core.DTOs;
using Displasrios.Recaudacion.Core.DTOs.Sales;
using Displasrios.Recaudacion.Core.Enums;
using Displasrios.Recaudacion.Core.Models.Sales;
using Displasrios.Recaudacion.Infraestructure.MainContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Displasrios.Recaudacion.Infraestructure.Repositories
{
    public class SaleRepository : ISaleRepository
    {
        private readonly DISPLASRIOSContext _context;

        public SaleRepository(DISPLASRIOSContext context)
        {
            _context = context;
        }

        public int Create(FullOrderDto order)
        {
            int idPedido = -1;
            int numeroPedido = -1;
            using (_context.Database.BeginTransaction())
            {
                //generación de secuencial de pedido y factura
                numeroPedido = (int)_context.Secuenciales.Where(x => x.Nombre.Equals("pedido"))
                                 .Select(x => x.Secuencial).FirstOrDefault() + 1;

                var pedido = new Factura
                {
                    UsuarioId = order.IdUser,
                    ClienteId = order.IdClient,
                    NumeroPedido = numeroPedido,
                    Secuencial = null,
                    Subtotal = order.Subtotal,
                    BaseImponible = order.Total * 1.12M,
                    Cambio = order.Change,
                    Iva = order.Iva,
                    Total = order.Total,
                    Etapa = (int)OrderStage.PENDIENTE_PAGO,
                    Subtotal0 = 0,
                    Subtotal12 = order.Subtotal,
                    Descuento = order.Discount,
                    FormaPago = order.PaymentMode,
                    MetodoPago = order.PaymentMethod,
                    PagoCliente = order.CustomerPayment,
                    Plazo = order.Deadline,
                    UsuarioCrea = order.Username,
                    CreadoEn = DateTime.Now,
                    Estado = 1
                };

                int nuevoSecuencial = 0;
                if (order.PaymentMethod == (int)FormaPago.CONTADO)
                {
                    nuevoSecuencial = (int)_context.Secuenciales.Where(x => x.Nombre.Equals("factura"))
                                 .Select(x => x.Secuencial).FirstOrDefault() + 1;
                    pedido.Secuencial = nuevoSecuencial;
                    pedido.FechaEmision = DateTime.Now;
                }

                _context.Factura.Add(pedido);
                _context.SaveChanges();
                idPedido = pedido.IdFactura;

                var pedidoDetalle = new List<FacturaDetalle>();
                foreach (var item in order.Items)
                {
                    var orderItem = new FacturaDetalle
                    {
                        FacturaId = idPedido,
                        ProductoId = item.Id,
                        Cantidad = item.Quantity,
                        PrecioUnitario = item.Price,
                        Descuento = 0,
                        Estado = true
                    };
                    pedidoDetalle.Add(orderItem);

                    var product = _context.Productos.Where(pro => pro.IdProducto == item.Id).FirstOrDefault();
                    product.Stock -= item.Quantity;
                }
                _context.FacturaDetalle.AddRange(pedidoDetalle);
                _context.SaveChanges();

                //establece el nuevo secuencial
                if (order.PaymentMethod == (int)FormaPago.CONTADO)
                {
                    var secuencialRow = _context.Secuenciales.First(x => x.Nombre.Equals("factura"));
                    secuencialRow.Secuencial = nuevoSecuencial;
                    _context.Secuenciales.Update(secuencialRow);
                }

                //registra abono
                if (order.CustomerPayment > 0) {
                    var pago = new Pagos
                    {
                        Fecha = DateTime.Now,
                        FacturaId = idPedido,
                        Pago = order.CustomerPayment,
                        Cambio = order.Change,
                        NumComprobantePago = order.NumPaymentReceipt
                    };
                    _context.Pagos.Add(pago);
                    _context.SaveChanges();
                }
                
                var numPedidoRow = _context.Secuenciales.First(x => x.Nombre.Equals("pedido"));
                numPedidoRow.Secuencial = numeroPedido;
                _context.Secuenciales.Update(numPedidoRow);
                _context.SaveChanges();

                _context.Database.CommitTransaction();
            }
            return idPedido > 0 ? numeroPedido : idPedido;
        }

        public IEnumerable<IncomeBySellersDto> GetIncomePerSellers(IncomeBySellers incomeBySellers)
        {
            var dateFrom = DateTime.Parse(incomeBySellers.DateFrom);
            var dateUntil = DateTime.Parse(incomeBySellers.DateUntil);

            var salesReport = _context.Factura.Where(x => x.Estado == 1 && x.Secuencial != null
            && x.CreadoEn.Date >= dateFrom.Date && x.CreadoEn.Date <= dateUntil.Date)
                .Include(fac => fac.Usuario)
                .GroupBy(group => group.Usuario.Usuario)
                .Select(x => new IncomeBySellersDto
                {
                    Total = x.Max(y => y.Total).ToString(),
                    User = x.Key
                }).ToArray();

            return salesReport;
        }
    }
}
