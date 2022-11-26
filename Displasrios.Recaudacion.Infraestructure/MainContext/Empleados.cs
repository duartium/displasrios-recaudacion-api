using System;
using System.Collections.Generic;

namespace Displasrios.Recaudacion.Infraestructure.MainContext
{
    public partial class Empleados
    {
        public Empleados()
        {
            PasswordReset = new HashSet<PasswordReset>();
        }

        public int IdEmpleado { get; set; }
        public string Identificacion { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public int TipoEmpleado { get; set; }
        public string Email { get; set; }
        public int Estado { get; set; }
        public DateTime CreadoEn { get; set; }
        public string UsuarioCrea { get; set; }
        public DateTime? ModificadoEn { get; set; }
        public string UsuarioMod { get; set; }
        public DateTime? EliminadoEn { get; set; }
        public string UsuarioElim { get; set; }
        public int? UsuarioId { get; set; }

        public virtual Usuarios Usuario { get; set; }
        public virtual ICollection<PasswordReset> PasswordReset { get; set; }
    }
}
