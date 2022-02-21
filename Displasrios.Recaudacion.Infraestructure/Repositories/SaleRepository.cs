using Displasrios.Recaudacion.Core.Contracts.Repositories;
using Displasrios.Recaudacion.Core.DTOs;
using Displasrios.Recaudacion.Core.Enums;
using Displasrios.Recaudacion.Infraestructure.MainContext;
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

            using (_context.Database.BeginTransaction())
            {
                //generación de secuencial de pedido y factura
                int numeroPedido = (int)_context.Secuenciales.Where(x => x.Nombre.Equals("pedido"))
                                 .Select(x => x.Secuencial).FirstOrDefault() + 1;

                var pedido = new Factura
                {
                    UsuarioId = order.IdUser,
                    ClienteId = order.IdClient,
                    FechaEmision = DateTime.Now,
                    NumeroPedido = numeroPedido,
                    Secuencial = null,
                    Subtotal = decimal.Parse(order.Subtotal, CultureInfo.InvariantCulture),
                    BaseImponible = decimal.Round(decimal.Parse(order.Total, CultureInfo.InvariantCulture) * 1.12M, 2),
                    Iva = decimal.Parse(order.Iva, CultureInfo.InvariantCulture),
                    Total = decimal.Parse(order.Total, CultureInfo.InvariantCulture),
                    Etapa = (int)OrderStage.PENDIENTE_PAGO,
                    Subtotal0 = 0,
                    Subtotal12 = decimal.Parse(order.Subtotal, CultureInfo.InvariantCulture),
                    Descuento = 0,
                    FormaPago = order.PaymentMethod,
                    PagoCliente = decimal.Parse(order.CustomerPayment, CultureInfo.InvariantCulture),
                    UsuarioCrea = order.Username,
                    CreadoEn = DateTime.Now,
                    Estado = 1
                };

                int nuevoSecuencial = 0;
                if (order.PaymentMode == (int)MetodoPago.CONTADO)
                {
                    nuevoSecuencial = (int)_context.Secuenciales.Where(x => x.Nombre.Equals("factura"))
                                 .Select(x => x.Secuencial).FirstOrDefault() + 1;
                    pedido.Secuencial = nuevoSecuencial;
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
                        PrecioUnitario = decimal.Parse(item.Price, CultureInfo.InvariantCulture),
                        Descuento = 0,
                        Estado = true
                    };
                    pedidoDetalle.Add(orderItem);
                }
                _context.FacturaDetalle.AddRange(pedidoDetalle);
                _context.SaveChanges();

                //establece el nuevo secuencial
                if (order.PaymentMode == (int)MetodoPago.CONTADO)
                {
                    var secuencialRow = _context.Secuenciales.First(x => x.Nombre.Equals("factura"));
                    secuencialRow.Secuencial = nuevoSecuencial;
                    _context.Secuenciales.Update(secuencialRow);
                }
                
                var numPedidoRow = _context.Secuenciales.First(x => x.Nombre.Equals("pedido"));
                numPedidoRow.Secuencial = numeroPedido;
                _context.Secuenciales.Update(numPedidoRow);
                _context.SaveChanges();

                _context.Database.CommitTransaction();
            }
            return idPedido;
        }
    }
}
