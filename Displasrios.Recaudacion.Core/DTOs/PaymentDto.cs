using System.Text.Json.Serialization;

namespace Displasrios.Recaudacion.Core.DTOs
{
    public class PaymentDto
    {
        [JsonPropertyName("date")]
        public string Date { get; set; }

        [JsonPropertyName("amount")]
        public decimal Amount { get; set; }
    }
}
