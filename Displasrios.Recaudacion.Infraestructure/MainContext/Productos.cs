using System;
using System.Collections.Generic;

namespace Displasrios.Recaudacion.Infraestructure.MainContext
{
    public partial class Productos
    {
        public Productos()
        {
            BajaProductos = new HashSet<BajaProductos>();
            EntradasDetalle = new HashSet<EntradasDetalle>();
            FacturaDetalle = new HashSet<FacturaDetalle>();
        }

        public int IdProducto { get; set; }
        public string Codigo { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public decimal Costo { get; set; }
        public decimal PrecioVenta { get; set; }
        public int Stock { get; set; }
        public int? CantXPaquete { get; set; }
        public int? CantXBulto { get; set; }
        public decimal? Descuento { get; set; }
        public bool Estado { get; set; }
        public int? TarifaIva { get; set; }
        public int? CategoriaId { get; set; }
        public int ProveedorId { get; set; }
        public DateTime CreadoEn { get; set; }
        public string UsuarioCrea { get; set; }
        public DateTime? ModificadoEn { get; set; }
        public string UsuarioMod { get; set; }
        public DateTime? EliminadoEn { get; set; }
        public string UsuarioElim { get; set; }

        public virtual Proveedores Proveedor { get; set; }
        public virtual ICollection<BajaProductos> BajaProductos { get; set; }
        public virtual ICollection<EntradasDetalle> EntradasDetalle { get; set; }
        public virtual ICollection<FacturaDetalle> FacturaDetalle { get; set; }
    }
}
