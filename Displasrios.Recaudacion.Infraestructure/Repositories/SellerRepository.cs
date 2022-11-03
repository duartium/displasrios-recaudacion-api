using Displasrios.Recaudacion.Core.Contracts.Repositories;
using Displasrios.Recaudacion.Core.DTOs;
using Displasrios.Recaudacion.Infraestructure.MainContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Displasrios.Recaudacion.Infraestructure.Repositories
{
    public class SellerRepository : ISellerRepository
    {
        private readonly DISPLASRIOSContext _context;
        public SellerRepository(DISPLASRIOSContext context)
        {
            _context = context;
        }

        public CustomerDebtDto GetDebts(int idSeller)
        {
            IQueryable<Factura> query = null;
            var debts = new CustomerDebtDto();


            query = _context.Factura.Where(x => x.Estado == 1 && x.Etapa == 1 && x.Secuencial == null
            );

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
