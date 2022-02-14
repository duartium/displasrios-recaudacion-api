using System;
using System.Collections.Generic;

namespace Displasrios.Recaudacion.Infraestructure.MainContext
{
    public partial class NotaCredito
    {
        public NotaCredito()
        {
            NotaCreditoDetalle = new HashSet<NotaCreditoDetalle>();
        }

        public int IdNotaCredito { get; set; }
        public int Secuencial { get; set; }
        public int UsuarioId { get; set; }
        public int ClienteId { get; set; }
        public int? FacturaId { get; set; }
        public DateTime FechaEmision { get; set; }
        public decimal BaseImponible { get; set; }
        public decimal Iva { get; set; }
        public decimal Iva0 { get; set; }
        public decimal? Subtotal { get; set; }
        public decimal Total { get; set; }
        public int Estado { get; set; }
        public string UsuarioCrea { get; set; }
        public DateTime CreadoEn { get; set; }
        public string UsuarioMod { get; set; }
        public DateTime? ModificadoEn { get; set; }
        public string UsuarioElim { get; set; }
        public DateTime? EliminadoEn { get; set; }

        public virtual ICollection<NotaCreditoDetalle> NotaCreditoDetalle { get; set; }
    }
}
