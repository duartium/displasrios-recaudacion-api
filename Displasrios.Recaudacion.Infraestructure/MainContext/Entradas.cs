using System;
using System.Collections.Generic;

namespace Displasrios.Recaudacion.Infraestructure.MainContext
{
    public partial class Entradas
    {
        public Entradas()
        {
            EntradasDetalle = new HashSet<EntradasDetalle>();
        }

        public int IdEntrada { get; set; }
        public DateTime FechaEmision { get; set; }
        public string NumComprobante { get; set; }
        public int? TipoComprobante { get; set; }
        public int? ProveedorId { get; set; }
        public string Observaciones { get; set; }
        public decimal TotalCompra { get; set; }
        public bool Estado { get; set; }
        public DateTime CreadoEn { get; set; }
        public string UsuarioCrea { get; set; }
        public DateTime? ModificadoEn { get; set; }
        public string UsuarioModifica { get; set; }
        public DateTime? EliminadoEn { get; set; }
        public string UsuarioElimina { get; set; }

        public virtual ICollection<EntradasDetalle> EntradasDetalle { get; set; }
    }
}
