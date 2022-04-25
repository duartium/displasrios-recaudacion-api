using System;
using System.Collections.Generic;

namespace Displasrios.Recaudacion.Infraestructure.MainContext
{
    public partial class FacturaDetalle
    {
        public int IdFacturaDetalle { get; set; }
        public int FacturaId { get; set; }
        public int ProductoId { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal Descuento { get; set; }
        public bool Estado { get; set; }

        public virtual Factura Factura { get; set; }
        public virtual Productos Producto { get; set; }
    }
}
