using System;
using System.Collections.Generic;

namespace Displasrios.Recaudacion.Infraestructure.MainContext
{
    public partial class Clientes
    {
        public Clientes()
        {
            Factura = new HashSet<Factura>();
        }

        public int IdCliente { get; set; }
        public string Identificacion { get; set; }
        public string TipoIdentificacion { get; set; }
        public int TipoCliente { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string Telefono { get; set; }
        public string Direccion { get; set; }
        public string Email { get; set; }
        public bool Estado { get; set; }
        public string UsuarioCrea { get; set; }
        public DateTime CreadoEn { get; set; }
        public string UsuarioMod { get; set; }
        public DateTime? ModificadoEn { get; set; }
        public string UsuarioElim { get; set; }
        public DateTime? EliminadoEn { get; set; }

        public virtual ICollection<Factura> Factura { get; set; }
    }
}
