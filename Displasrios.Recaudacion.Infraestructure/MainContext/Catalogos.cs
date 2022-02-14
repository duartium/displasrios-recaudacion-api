using System;
using System.Collections.Generic;

namespace Displasrios.Recaudacion.Infraestructure.MainContext
{
    public partial class Catalogos
    {
        public Catalogos()
        {
            ItemCatalogo = new HashSet<ItemCatalogo>();
        }

        public int IdCatalogo { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public int Tipo { get; set; }
        public bool Estado { get; set; }
        public DateTime CreadoEn { get; set; }
        public string UsuarioCrea { get; set; }
        public DateTime? ModificadoEn { get; set; }
        public string UsuarioMod { get; set; }
        public DateTime? EliminadoEn { get; set; }
        public string UsuarioElim { get; set; }

        public virtual ICollection<ItemCatalogo> ItemCatalogo { get; set; }
    }
}
