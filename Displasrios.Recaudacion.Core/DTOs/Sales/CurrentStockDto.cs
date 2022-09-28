using System.Text.Json.Serialization;

namespace Displasrios.Recaudacion.Core.DTOs.Sales
{
    public class CurrentStockDto
    {
        [JsonPropertyName("currentStock")]
        public int CurrentStock { get; set; }

        [JsonPropertyName("enoughtStock")]
        public int EnoughtStock { get; set; }
    }
}
