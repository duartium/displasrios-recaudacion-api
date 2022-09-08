using System;
using System.Collections.Generic;

namespace Displasrios.Recaudacion.Infraestructure.MainContext
{
    public partial class MovimientosCaja
    {
        public int IdMovimiento { get; set; }
        public DateTime Fecha { get; set; }
        public short? TipoMovimiento { get; set; }
        public decimal? MontoRecibido { get; set; }
        public decimal? MontoSistema { get; set; }
        public decimal? Diferencia { get; set; }
        public decimal? SaldoCaja { get; set; }
        public string Observaciones { get; set; }
        public int UsuarioId { get; set; }

        public virtual Usuarios Usuario { get; set; }
    }
}
