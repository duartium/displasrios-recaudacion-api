using System.ComponentModel.DataAnnotations;

namespace Displasrios.Recaudacion.Core.DTOs.Reports
{
    public class BestCustomersDto
    {
        [Key]
        public int IdCliente { get; set; }
        public decimal Totales { get; set; }
    }
}
