using System;
using System.Collections.Generic;

namespace Displasrios.Recaudacion.Infraestructure.MainContext
{
    public partial class Ingresos
    {
        public int IdIngresos { get; set; }
        public DateTime Fecha { get; set; }
        public decimal Valor { get; set; }
        public int UsuarioId { get; set; }
        public string Observaciones { get; set; }
        public int Estado { get; set; }
        public string UsuarioCrea { get; set; }
        public DateTime? FechaModifica { get; set; }
        public string UsuarioModifica { get; set; }

        public virtual Usuarios Usuario { get; set; }
    }
}
