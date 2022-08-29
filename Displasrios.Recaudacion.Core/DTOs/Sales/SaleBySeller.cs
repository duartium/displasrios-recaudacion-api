using System.Text.Json.Serialization;

namespace Displasrios.Recaudacion.Core.DTOs.Sales
{
    public class IncomeBySellersDto
    {
        [JsonPropertyName("total")]
        public string Total { get; set; }

        [JsonPropertyName("user")]
        public string User { get; set; }
    }
}