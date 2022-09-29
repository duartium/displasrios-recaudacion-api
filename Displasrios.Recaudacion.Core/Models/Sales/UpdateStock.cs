using System.Text.Json.Serialization;

namespace Displasrios.Recaudacion.Core.Models.Sales
{
    public class UpdateStock
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("quantity")]
        public int Quantity { get; set; }
    }
}
