using System;
using System.Collections.Generic;

namespace Displasrios.Recaudacion.Infraestructure.MainContext
{
    public partial class Factura
    {
        public Factura()
        {
            FacturaDetalle = new HashSet<FacturaDetalle>();
            Pagos = new HashSet<Pagos>();
            Visitas = new HashSet<Visitas>();
        }

        public int IdFactura { get; set; }
        public int? Secuencial { get; set; }
        public int NumeroPedido { get; set; }
        public int UsuarioId { get; set; }
        public int ClienteId { get; set; }
        public int Etapa { get; set; }
        public DateTime FechaEmision { get; set; }
        public decimal BaseImponible { get; set; }
        public decimal Iva { get; set; }
        public decimal Subtotal0 { get; set; }
        public decimal Subtotal12 { get; set; }
        public decimal Subtotal { get; set; }
        public decimal Total { get; set; }
        public decimal Descuento { get; set; }
        public int FormaPago { get; set; }
        public int MetodoPago { get; set; }
        public int Plazo { get; set; }
        public decimal? PagoCliente { get; set; }
        public decimal? Cambio { get; set; }
        public int Estado { get; set; }
        public string UsuarioCrea { get; set; }
        public DateTime CreadoEn { get; set; }
        public string UsuarioMod { get; set; }
        public DateTime? ModificadoEn { get; set; }
        public string UsuarioElim { get; set; }
        public DateTime? EliminadoEn { get; set; }
        public string MotivoAnulacion { get; set; }

        public virtual Clientes Cliente { get; set; }
        public virtual Usuarios Usuario { get; set; }
        public virtual ICollection<FacturaDetalle> FacturaDetalle { get; set; }
        public virtual ICollection<Pagos> Pagos { get; set; }
        public virtual ICollection<Visitas> Visitas { get; set; }
    }
}
