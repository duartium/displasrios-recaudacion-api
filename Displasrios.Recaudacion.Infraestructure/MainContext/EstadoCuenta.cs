using System;
using System.Collections.Generic;

namespace Displasrios.Recaudacion.Infraestructure.MainContext
{
    public partial class EstadoCuenta
    {
        public int IdEstadoCuenta { get; set; }
        public int ClienteId { get; set; }
        public int UsuarioId { get; set; }
        public DateTime Fecha { get; set; }
        public DateTime? Vence { get; set; }
        public int? DiasMora { get; set; }
        public decimal TotalFactura { get; set; }
        public decimal? TotalAbono { get; set; }
        public decimal? TotalSaldo { get; set; }

        public virtual Usuarios Usuario { get; set; }
    }
}
