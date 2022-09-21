using System.Text.Json.Serialization;

namespace Displasrios.Recaudacion.Core.DTOs
{
    public class TotalCashCloseDto
    {
        [JsonPropertyName("totalSellersIncome")]
        public decimal TotalSellersIncome { get; set; }

        [JsonPropertyName("totalLocal")]
        public decimal TotalLocal { get; set; }

        [JsonPropertyName("totalExpenses")]
        public decimal TotalExpenses { get; set; }

        [JsonPropertyName("totalEgresos")]
        public decimal TotalEgresos { get; set; }
    }
}