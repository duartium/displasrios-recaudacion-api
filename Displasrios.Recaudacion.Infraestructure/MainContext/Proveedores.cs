using System;
using System.Collections.Generic;

namespace Displasrios.Recaudacion.Infraestructure.MainContext
{
    public partial class Proveedores
    {
        public Proveedores()
        {
            Productos = new HashSet<Productos>();
        }

        public int IdProveedor { get; set; }
        public string Ruc { get; set; }
        public string Nombre { get; set; }
        public string Direccion { get; set; }
        public string Email { get; set; }
        public string Telefono { get; set; }
        public bool Estado { get; set; }
        public DateTime CreadoEn { get; set; }
        public string UsuarioCrea { get; set; }
        public DateTime? ModificadoEn { get; set; }
        public string UsuarioMod { get; set; }
        public DateTime? EliminadoEn { get; set; }
        public string UsuarioElim { get; set; }
        public bool? ProveedorDefault { get; set; }

        public virtual ICollection<Productos> Productos { get; set; }
    }
}
