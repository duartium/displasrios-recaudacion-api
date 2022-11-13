using Displasrios.Recaudacion.Core.Constants;
using Displasrios.Recaudacion.Core.Contracts.Repositories;
using Displasrios.Recaudacion.Core.DTOs;
using Displasrios.Recaudacion.Core.DTOs.Sales;
using Displasrios.Recaudacion.Core.Enums;
using Displasrios.Recaudacion.Core.Models;
using Displasrios.Recaudacion.Infraestructure.MainContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
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
            var orderReceivable = _context.Factura.Where(x => x.Estado == 1 && x.IdFactura == idOrder)
                .Include(detail => detail.FacturaDetalle).ThenInclude(product => product.Producto)
                .Include(paymenths => paymenths.Pagos)
                .Include(client => client.Cliente)
                .Select(order => new OrderReceivableDto
                {
                    Date = order.FechaEmision.ToString("dd/MM/yyyy"),
                    OrderNumber = order.NumeroPedido.ToString().PadLeft(5, '0'),
                    InvoiceNumber = String.IsNullOrEmpty(order.Secuencial.ToString()) ? "" : order.Secuencial.ToString().PadLeft(5, '0'),
                    DaysDebt = (DateTime.Now - order.FechaEmision).Days,
                    TotalAmount = order.Total,
                    Iva = order.Iva,
                    Subtotal = order.Subtotal,
                    Subtotal0 = order.Subtotal0,
                    Subtotal12 = order.Subtotal12 - order.Iva,
                    Discount = order.Descuento,
                    FullNames = order.Cliente.Nombres.ToUpper() + " " + order.Cliente.Apellidos.ToUpper(),
                    Deadline = order.Plazo,
                    WayToPay = order.MetodoPago.ToString(),
                    PaymentMethod = order.FormaPago.ToString(),
                    Payments = order.Pagos.Select(x => new PaymentDto
                    {
                        Amount = x.PagoReal,
                        Date = x.Fecha.ToString("dd/MM/yyyy")
                    }).ToArray(),
                    Products = order.FacturaDetalle.Select(det => new ProductResumeDto
                    {
                        Name = det.Producto.Nombre,
                        Price = det.PrecioUnitario,
                        Quantity = det.Cantidad,
                        Total = Math.Round(det.Cantidad * det.PrecioUnitario, 2)
                    }).ToArray(),
                    CollectorName = order.UsuarioCrea
                }).FirstOrDefault();

            orderReceivable.WayToPay = _context.ItemCatalogo.Where(x => x.Estado && x.IdItemCatalogo == int.Parse(orderReceivable.WayToPay))
                .Select(x => x.Descripcion).First();

            orderReceivable.PaymentMethod = _context.ItemCatalogo.Where(x => x.Estado && x.IdItemCatalogo == int.Parse(orderReceivable.PaymentMethod))
                .Select(x => x.Descripcion).First();

            orderReceivable.Visits = _context.Visitas.Where(x => x.OrderId == idOrder)
                .Select(visit => new VisitDto { 
                    Date= visit.Fecha.ToString("dd/MM/yyyy"),
                    Observations = visit.Observacion,
                    Username = visit.UsuarioVisita
                }).ToArray();

            orderReceivable.Balance = orderReceivable.Payments.Length > 0
                ? orderReceivable.TotalAmount - orderReceivable.Payments.Sum(x => x.Amount) : orderReceivable.TotalAmount; 

            return orderReceivable;
        }

        public bool RegisterPayment(OrderReceivableCreateRequest order, out string mensaje)
        {
            
            using (var trans = _context.Database.BeginTransaction())
            {
                //registra el nuevo pago
                var pago = new Pagos
                {
                    Fecha = DateTime.Now,
                    FacturaId = order.IdOrder,
                    Pago = order.CustomerPayment,
                    PagoReal = order.CustomerPayment - order.Change,
                    Cambio = order.Change
                };
                _context.Pagos.Add(pago);
                _context.SaveChanges();

                mensaje = $"Se registró el abono de ${String.Format("{0:0.00}", pago.PagoReal)}.";

                var totals = _context.Factura.Where(x => x.Estado == 1 && x.IdFactura == order.IdOrder)
                    .Include(order => order.Pagos)
                    .Select(order => new PaymentsTotal { 
                        TotalOrder = order.Total,
                        TotalPaymentsCurrent = order.Pagos.Select(x => x.PagoReal).Sum()
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
                    invoice.Etapa = (int)OrderStage.PAGADO;
                    _context.Factura.Update(invoice);
                    _context.SaveChanges();

                    mensaje = $"Se generó la factura Nº{nuevoSecuencial.ToString().PadLeft(5, '0')}";
                }
                
                _context.Database.CommitTransaction();
            }

            return true;
        }

        public IEnumerable<OrderSummaryDto> GetOrdersReceivable(FiltersOrdersReceivable filters)
        {
            return _context.Factura.Where(x => x.Estado == 1 && x.Etapa == 1 && x.Secuencial == null 
                    && x.UsuarioId == filters.IdUser)
                .Include(client => client.Cliente)
                .Include(pagos => pagos.Pagos)
                .Select(order => new OrderSummaryDto { 
                    Id = order.IdFactura,
                    OrderNumber = order.NumeroPedido.ToString().PadLeft(5, '0'),
                    FullNames = order.Cliente.Nombres + " " + order.Cliente.Apellidos,
                    TotalAmount = order.Pagos.Count > 0 
                        ? Math.Round(order.Total - order.Pagos.Sum(x => x.PagoReal), 2).ToString() : order.Total.ToString(),
                    Date = order.FechaEmision.ToString("dd/MM/yyyy")
                }).ToList().OrderBy(x => x.OrderNumber).ToList();
        }

        public IEnumerable<SummaryOrdersOfDay> GetSummaryOrdersOfDay()
        {
            var orders = _context.Factura.Where(x => x.Estado == 1 && x.FechaEmision.Date == DateTime.Now.Date.Date)
                .Include(fac => fac.Cliente)
                .Select(x => new SummaryOrdersOfDay {
                    IdOrder = x.IdFactura,
                    Date = x.CreadoEn.ToString("dd-MM-yyyy HH:mm"),
                    OrderNumber = x.NumeroPedido.ToString().PadLeft(5, '0'),
                    Stage =  x.Etapa.ToString(),
                    FullNames = x.Cliente.Nombres + " " + x.Cliente.Apellidos,
                    TotalAmount = x.Total,
                    Username = x.UsuarioCrea
                }).ToList().OrderByDescending(x => x.OrderNumber);

            orders.ToList().ForEach(x => x.Stage = CValues.ETAPA_PEDIDO[int.Parse(x.Stage) - 1]);
            return orders;
        }

        public IEnumerable<SummaryOrdersOfDay> GetCollectionOfDay()
        {
            var orders = _context.Factura.Where(x => x.Estado == 1 && x.FechaEmision.Date == DateTime.Now.Date.Date
            && x.Secuencial == null)
                .Include(pag => pag.Pagos)
                .Include(fac => fac.Cliente)
                .Select(x => new SummaryOrdersOfDay
                {
                    IdOrder = x.IdFactura,
                    Date = x.CreadoEn.ToString("dd-MM-yyyy HH:mm"),
                    OrderNumber = x.NumeroPedido.ToString().PadLeft(5, '0'),
                    Stage = x.Etapa.ToString(),
                    FullNames = x.Cliente.Nombres + " " + x.Cliente.Apellidos,
                    TotalAmount = x.Total - x.Pagos.Where(y => y.FacturaId == x.IdFactura).Sum(x => x.PagoReal),
                    Username = x.UsuarioCrea
                }).ToList().OrderByDescending(x => x.OrderNumber);

            orders.ToList().ForEach(x => x.Stage = CValues.ETAPA_PEDIDO[int.Parse(x.Stage) - 1]);
            return orders;
        }


        public IEnumerable<SummaryOrdersOfDay> GetSummaryOrdersByCustomer(int idCustomer)
        {
            var orders = _context.Factura.Where(x => x.Estado == 1 && x.ClienteId == idCustomer)
                .Include(fac => fac.Cliente)
                .Select(x => new SummaryOrdersOfDay
                {
                    IdOrder = x.IdFactura,
                    Date = x.CreadoEn.ToString("dd-MM-yyyy HH:mm"),
                    OrderNumber = x.NumeroPedido.ToString().PadLeft(5, '0'),
                    Stage = x.Etapa.ToString(),
                    FullNames = x.Cliente.Nombres + " " + x.Cliente.Apellidos,
                    TotalAmount = x.Total,
                    Username = x.UsuarioCrea
                }).ToList().OrderByDescending(x => x.OrderNumber);

            orders.ToList().ForEach(x => x.Stage = CValues.ETAPA_PEDIDO[int.Parse(x.Stage) - 1]);
            return orders;
        }

        public SummaryOrdersOfDay GetSummaryOrder(int orderNumber)
        {
            var order = _context.Factura.Where(x => x.Estado == 1 && x.NumeroPedido == orderNumber)
                .Include(fac => fac.Cliente)
                .Select(x => new SummaryOrdersOfDay
                {
                    IdOrder = x.IdFactura,
                    Date = x.CreadoEn.ToString("dd-MM-yyyy HH:mm"),
                    OrderNumber = x.NumeroPedido.ToString().PadLeft(5, '0'),
                    Stage = x.Etapa.ToString(),
                    FullNames = x.Cliente.Nombres + " " + x.Cliente.Apellidos,
                    TotalAmount = x.Total,
                    Username = x.UsuarioCrea
                }).FirstOrDefault();

            order.Stage = CValues.ETAPA_PEDIDO[int.Parse(order.Stage) - 1];
            return order;
        }

        public bool RecordVisit(VisitCreation visitCreation) {
            var visit = new Visitas { 
                Fecha = DateTime.Now,
                UsuarioVisita = visitCreation.Username,
                OrderId = visitCreation.OrderId,
                Observacion = visitCreation.Observations
            };
            _context.Visitas.Add(visit);
            int rowsAffected = _context.SaveChanges();
            return rowsAffected > 0;
        }

        public bool CancelOrder(int idOrder, string username)
        {
            var order = _context.Factura.Where(x => x.IdFactura == idOrder).FirstOrDefault();
            order.Estado = (int)OrderStage.ANULADO;
            order.UsuarioElim = username;
            order.EliminadoEn = DateTime.Now;
            order.Etapa = (int)OrderStage.ANULADO;

            _context.Update(order);
            int rowsAffected = _context.SaveChanges();
            return rowsAffected > 0;
        }

        public decimal GetTotalSalesTodayBySeller(int idUser) {
            decimal totalSalesToday = _context.Factura
                .Include(fac => fac.Usuario).Where(x => x.Estado == 1 && x.Secuencial != null
            && x.CreadoEn.Date == DateTime.Now.Date && x.UsuarioId == idUser)
                .Select(x => x.Total).Sum();

            return totalSalesToday;
        }

        public SellerPersonalReportDto GetSellerPersonalReport(int idUser)
        {
            var totalsReport = new SellerPersonalReportDto();

            totalsReport.TotalSalesToday = _context.Factura.Where(x => x.Estado == 1 && x.Secuencial != null
            && x.CreadoEn.Date == DateTime.Now.Date && x.UsuarioId == idUser)
                .Select(x => x.Total).Sum();

            totalsReport.totalMonthSales = _context.Factura.Where(x => x.Estado == 1 && x.Secuencial != null
            && x.CreadoEn.Date.Month == DateTime.Now.Month && x.CreadoEn.Date.Year == DateTime.Now.Year 
            && x.UsuarioId == idUser)
                .Select(x => x.Total).Sum();

            totalsReport.NumOrdersReceivable = _context.Factura.Where(x => x.Estado == 1 && x.Secuencial == null
            && x.UsuarioId == idUser).Count();

            return totalsReport;
        }

        public CustomerDebtDto GetSellerDebts(int id)
        {
            IQueryable<Factura> query = null;
            var debts = new CustomerDebtDto();

            int idEmployee = (int)_context.Empleados.Where(x => x.IdEmpleado == id)
                .Select(x => x.UsuarioId).FirstOrDefault();

            query = _context.Factura.Where(x => x.Estado == 1 && x.Etapa == 1 && x.Secuencial == null
                    && x.UsuarioId == idEmployee);

            debts.OrdersReceivable = query.Include(client => client.Cliente).Include(pagos => pagos.Pagos)
                .Include(fac => fac.Usuario)
               .Select(order => new OrderSummaryDto
               {
                   Id = order.IdFactura,
                   OrderNumber = order.NumeroPedido.ToString().PadLeft(5, '0'),
                   FullNames = order.Cliente.Nombres + " " + order.Cliente.Apellidos,
                   Collector = order.Usuario.Usuario,
                   TotalAmount = order.Pagos.Count > 0
                       ? Math.Round(order.Total - order.Pagos.Sum(x => x.PagoReal), 2).ToString() : order.Total.ToString(),
                   Date = order.FechaEmision.ToString("dd/MM/yyyy")
               }).ToList();

            if (debts.OrdersReceivable != null && debts.OrdersReceivable.Count > 0)
            {
                debts.Fullnames = debts.OrdersReceivable.FirstOrDefault().FullNames.ToUpper();
                debts.TotalDebts = debts.OrdersReceivable.Sum(x => decimal.Parse(x.TotalAmount, CultureInfo.InvariantCulture));
            }

            return debts;
        }
    }
}
