using Displasrios.Recaudacion.Core.Contracts.Repositories;
using Displasrios.Recaudacion.Core.Models;
using Displasrios.Recaudacion.Infraestructure.MainContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Displasrios.Recaudacion.Infraestructure.Repositories
{
    public class PurchaseRepository : IPurchaseRepository
    {
        private readonly DISPLASRIOSContext _context;

        public PurchaseRepository(DISPLASRIOSContext context)
        {
            _context = context;
        }

        public bool Register(PurchaseCreate purchase)
        {
            int resp = -1;
            using (_context.Database.BeginTransaction())
            {
                var newPurchase = new Entradas
                {
                    NumComprobante = purchase.NumInvoice.Trim().Length == 0 ? null : purchase.NumInvoice,
                    TipoComprobante = 1,
                    ProveedorId = purchase.IdProvider,
                    Observaciones = purchase.Observations.Trim().Length == 0 ? null : purchase.Observations,
                    TotalCompra = purchase.Total,
                    FechaEmision = Convert.ToDateTime(purchase.Date),
                    UsuarioCrea = purchase.UserCreation,
                    CreadoEn = DateTime.Now,
                    Estado = true
                };

                _context.Entradas.Add(newPurchase);
                resp = _context.SaveChanges();

                //actualiza el stock de cada producto
                var details = new List<EntradasDetalle>();
                foreach (var item in purchase.Items)
                {
                    var product = _context.Productos.Where(x => x.Estado && x.IdProducto == item.Id).First();

                    //se almacena en memoria cada item de detalle
                    var detail = new EntradasDetalle
                    {
                        EntradaId = newPurchase.IdEntrada,
                        ProductoId = item.Id,
                        Cantidad = item.Quantity,
                        Precio = item.Price,
                        StockActual = product.Stock,
                        Estado = true
                    };

                    product.Stock += item.Quantity;
                    detail.StockNuevo = product.Stock;

                    _context.Entry(product).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    _context.SaveChanges();
                    details.Add(detail);
                }
                _context.EntradasDetalle.AddRange(details);
                _context.SaveChanges();

                _context.Database.CommitTransaction();
            }
            return resp > 0;
        }

    }
}
