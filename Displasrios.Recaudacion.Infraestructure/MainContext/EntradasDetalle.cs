using System;
using System.Collections.Generic;

namespace Displasrios.Recaudacion.Infraestructure.MainContext
{
    public partial class EntradasDetalle
    {
        public int IdEntradaDetalle { get; set; }
        public int? EntradaId { get; set; }
        public int? ProductoId { get; set; }
        public int? StockActual { get; set; }
        public int? StockNuevo { get; set; }
        public int? Cantidad { get; set; }
        public decimal? Precio { get; set; }
        public bool? Estado { get; set; }

        public virtual Entradas Entrada { get; set; }
        public virtual Productos Producto { get; set; }
    }
}
