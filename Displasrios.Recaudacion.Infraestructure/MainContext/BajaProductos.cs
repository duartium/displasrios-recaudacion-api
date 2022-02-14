using System;
using System.Collections.Generic;

namespace Displasrios.Recaudacion.Infraestructure.MainContext
{
    public partial class BajaProductos
    {
        public int IdBaja { get; set; }
        public int ProductoId { get; set; }
        public int MotivoId { get; set; }
        public int Cantidad { get; set; }
        public string OtroMotivo { get; set; }
        public DateTime Fecha { get; set; }
        public string Usuario { get; set; }

        public virtual Productos Producto { get; set; }
    }
}
