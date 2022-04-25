using System;
using System.Collections.Generic;

namespace Displasrios.Recaudacion.Infraestructure.MainContext
{
    public partial class Pagos
    {
        public int IdPago { get; set; }
        public int FacturaId { get; set; }
        public decimal Pago { get; set; }
        public decimal Cambio { get; set; }
        public DateTime Fecha { get; set; }

        public virtual Factura Factura { get; set; }
    }
}
