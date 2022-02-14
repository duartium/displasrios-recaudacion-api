using System;
using System.Collections.Generic;

namespace Displasrios.Recaudacion.Infraestructure.MainContext
{
    public partial class ItemCatalogo
    {
        public int IdItemCatalogo { get; set; }
        public int? CatalogoId { get; set; }
        public string Codigo { get; set; }
        public string Descripcion { get; set; }
        public bool Estado { get; set; }
        public DateTime CreadoEn { get; set; }
        public string UsuarioCrea { get; set; }
        public DateTime? ModificadoEn { get; set; }
        public string UsuarioMod { get; set; }
        public DateTime? EliminadoEn { get; set; }
        public string UsuarioElim { get; set; }

        public virtual Catalogos Catalogo { get; set; }
    }
}
