using System;
using System.Collections.Generic;

namespace Displasrios.Recaudacion.Infraestructure.MainContext
{
    public partial class Usuarios
    {
        public Usuarios()
        {
            Empleados = new HashSet<Empleados>();
            EstadoCuenta = new HashSet<EstadoCuenta>();
            Factura = new HashSet<Factura>();
            MovimientosCaja = new HashSet<MovimientosCaja>();
        }

        public int IdUsuario { get; set; }
        public string Usuario { get; set; }
        public string Clave { get; set; }
        public int PerfilId { get; set; }
        public bool Estado { get; set; }
        public DateTime CreadoEn { get; set; }
        public string UsuarioCrea { get; set; }
        public DateTime? ModificadoEn { get; set; }
        public string UsuarioMod { get; set; }
        public DateTime? EliminadoEn { get; set; }
        public string UsuarioElim { get; set; }

        public virtual ICollection<Empleados> Empleados { get; set; }
        public virtual ICollection<EstadoCuenta> EstadoCuenta { get; set; }
        public virtual ICollection<Factura> Factura { get; set; }
        public virtual ICollection<MovimientosCaja> MovimientosCaja { get; set; }
    }
}
