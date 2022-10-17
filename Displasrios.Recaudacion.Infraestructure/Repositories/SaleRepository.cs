using Displasrios.Recaudacion.Core.Constants;
using Displasrios.Recaudacion.Core.Contracts.Repositories;
using Displasrios.Recaudacion.Core.DTOs;
using Displasrios.Recaudacion.Core.DTOs.Sales;
using Displasrios.Recaudacion.Core.Enums;
using Displasrios.Recaudacion.Core.Models.Sales;
using Displasrios.Recaudacion.Infraestructure.MainContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Displasrios.Recaudacion.Infraestructure.Repositories
{
    public class SaleRepository : ISaleRepository
    {
        private readonly DISPLASRIOSContext _context;

        public SaleRepository(DISPLASRIOSContext context)
        {
            _context = context;
        }

        public SaleCreated Create(FullOrderDto order)
        {
            var result = new SaleCreated() { SendEmail = false };

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
                    FechaEmision = DateTime.Now,
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
                    pedido.Etapa = (int)OrderStage.PAGADO;
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
                    _context.Update(product);
                    _context.SaveChanges();
                }
                _context.FacturaDetalle.AddRange(pedidoDetalle);
                _context.SaveChanges();

                //establece el nuevo secuencial a factura
                if (order.PaymentMethod == (int)FormaPago.CONTADO)
                {
                    var secuencialRow = _context.Secuenciales.First(x => x.Nombre.Equals("factura"));
                    secuencialRow.Secuencial = nuevoSecuencial;
                    _context.Secuenciales.Update(secuencialRow);

                    result.SendEmail = true;
                }

                //registra abono/pago
                if (order.CustomerPayment > 0) {
                    var pago = new Pagos
                    {
                        Fecha = DateTime.Now,
                        FacturaId = idPedido,
                        Pago = order.CustomerPayment,
                        PagoReal = order.CustomerPayment - order.Change,
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

            result.OrderNumber = numeroPedido;
            return result;
        }

        public IEnumerable<IncomeBySellersDto> GetIncomePerSellers(IncomeBySellers incomeBySellers)
        {
            var dateFrom = DateTime.Parse(incomeBySellers.DateFrom);
            var dateUntil = DateTime.Parse(incomeBySellers.DateUntil);

            //obtiene todos los usuarios recaudadores y los inicializa con cero
            var allUsers = _context.Usuarios.Where(x => x.Estado && x.PerfilId == 1)
                .Select(x => new IncomeBySellersDto { 
                    Total = "0",
                    User = x.Usuario
                }).ToList();

            var salesReport = _context.Factura.Where(x => x.Estado == 1 && x.Secuencial != null
            && x.CreadoEn.Date >= dateFrom.Date && x.CreadoEn.Date <= dateUntil.Date)
                .Include(fac => fac.Usuario)
                .GroupBy(group => group.Usuario.Usuario)
                .Select(x => new IncomeBySellersDto
                {
                    Total = x.Sum(y => y.Total).ToString(),
                    User = x.Key
                }).ToArray();

            foreach (var sales in salesReport)
            {
                var user = allUsers.Where(x => x.User == sales.User).FirstOrDefault();
                if(user != null)
                {
                    allUsers.Where(x => x.User == sales.User).First().Total = sales.Total;
                }
            }

            return allUsers;
        }

        public bool SaveCollectorSale(SalesSellerToday salesSellerToday)
        {
            var salesSeller = new Ingresos
            {
                UsuarioId = salesSellerToday.IdUser,
                Valor = salesSellerToday.Amount,
                Estado = 1,
                Fecha = DateTime.Now,
                UsuarioCrea = salesSellerToday.Username
            };

            _context.Ingresos.Add(salesSeller);
            int resp = _context.SaveChanges();
            return resp > 0;
        }

        public string GetSaleTemplateForEmail(int orderNumber)
        {
            var sale = _context.Factura.Where(x => x.Estado == 1 && x.NumeroPedido == orderNumber)
                .Include(detail => detail.FacturaDetalle).ThenInclude(product => product.Producto)
                .Include(client => client.Cliente)
                .Select(order => new OrderEmail
                {
                    Date = order.FechaEmision.ToString("dd/MM/yyyy HH:mm"),
                    OrderNumber = order.NumeroPedido.ToString().PadLeft(5, '0'),
                    InvoiceNumber = order.Secuencial.ToString().PadLeft(5, '0'),
                    TotalAmount = order.Total,
                    Iva = order.Iva,
                    Subtotal = order.Subtotal,
                    Subtotal0 = order.Subtotal0,
                    Subtotal12 = order.Subtotal12 - order.Iva,
                    Discount = order.Descuento,
                    Address = order.Cliente.Direccion,
                    Email = order.Cliente.Email,
                    FullNames = order.Cliente.Nombres.ToUpper() + " " + order.Cliente.Apellidos.ToUpper(),
                    Products = order.FacturaDetalle.Select(det => new ProductResumeDto
                    {
                        Name = det.Producto.Nombre,
                        Price = det.PrecioUnitario,
                        Quantity = det.Cantidad,
                        Total = Math.Round(det.Cantidad * det.PrecioUnitario, 2)
                    }).ToArray(),
                }).FirstOrDefault();

            string templateEmail = CString.RECIBO_TEMPLATE.Replace("@nombreCliente", sale.FullNames)
                .Replace("@direccion", sale.Address).Replace("@email", sale.Email)
                .Replace("@orderNumber", sale.OrderNumber).Replace("@numInvoice", sale.InvoiceNumber)
                .Replace("@orderNumber", sale.OrderNumber).Replace("@fechaEmision", sale.Date)
                .Replace("@subtotal", String.Format("{0:0.00}", Math.Round(sale.Subtotal * 1.12M, 2))
                .Replace("@eliva", String.Format("{0:0.00}", sale.Iva))
                .Replace("@descuento", String.Format("{0:0.00}", sale.Discount))
                .Replace("@total", String.Format("{0:0.00}", sale.TotalAmount)))
                .Replace("@heigthWrapper", (550 + (sale.Products.Count() * 5)).ToString() + "px")
                .Replace("@heigthChild", (420 + (sale.Products.Count() * 5)).ToString() + "px");

            string itemInvoice = "<table border='0' cellspacing='0' cellpadding='0' style='width:100%'>";
            foreach (var product in sale.Products)
            {
                itemInvoice += ($"<tr>"+
                    $"<td style='width:10%; text-align:center'>{product.Quantity}</td>" +
                    $"<td style='width:70%; text-align:left'>{product.Name}</td>" +
                    $"<td style='width:20%; text-align:right'>${String.Format("{0:0.00}", product.Total)}</td></tr>");
            }
            itemInvoice += "</table>";
            templateEmail = templateEmail.Replace("@details", itemInvoice);

            return templateEmail;
        }

        public string GetEmailFromInvoice(int orderNumber)
        {
            return _context.Factura.Where(x => x.Estado == 1 && x.NumeroPedido == orderNumber)
                .Include(client => client.Cliente).Select(x => x.Cliente.Email).FirstOrDefault();
        }

    }
}
