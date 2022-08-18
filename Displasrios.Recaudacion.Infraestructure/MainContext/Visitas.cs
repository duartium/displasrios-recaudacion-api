using System;
using System.Collections.Generic;

namespace Displasrios.Recaudacion.Infraestructure.MainContext
{
    public partial class Visitas
    {
        public int IdVisita { get; set; }
        public DateTime Fecha { get; set; }
        public string Observacion { get; set; }
        public string UsuarioVisita { get; set; }
        public int OrderId { get; set; }

        public virtual Factura Order { get; set; }
    }
}
