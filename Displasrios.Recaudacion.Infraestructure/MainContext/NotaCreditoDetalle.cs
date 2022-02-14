using System;
using System.Collections.Generic;

namespace Displasrios.Recaudacion.Infraestructure.MainContext
{
    public partial class NotaCreditoDetalle
    {
        public int IdNotaCreditoDetalle { get; set; }
        public int NotaCreditoId { get; set; }
        public int? ProductoId { get; set; }
        public int? Cantidad { get; set; }
        public decimal? PrecioUnitario { get; set; }
        public string RazonModificacion { get; set; }
        public decimal ValorModificacion { get; set; }
        public bool Estado { get; set; }

        public virtual NotaCredito NotaCredito { get; set; }
    }
}
