using System;
using System.Collections.Generic;

namespace Displasrios.Recaudacion.Infraestructure.MainContext
{
    public partial class PasswordReset
    {
        public int IdVerificacion { get; set; }
        public DateTime Fecha { get; set; }
        public string Email { get; set; }
        public string CodigoVerificacion { get; set; }
        public DateTime? ActualizadoEn { get; set; }
        public int EmpleadoId { get; set; }

        public virtual Empleados Empleado { get; set; }
    }
}
