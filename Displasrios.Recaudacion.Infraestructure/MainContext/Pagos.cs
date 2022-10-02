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
        public decimal PagoReal { get; set; }
        public DateTime Fecha { get; set; }
        public string NumComprobantePago { get; set; }
        public string ImagenComprobante { get; set; }
        public string Usuario { get; set; }

        public virtual Factura Factura { get; set; }
    }
}
