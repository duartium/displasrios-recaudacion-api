using System.Text.Json.Serialization;

namespace Displasrios.Recaudacion.Core.Models.Sales
{
    public class IncomeBySellers
    {
        [JsonPropertyName("dateFrom")]
        public string DateFrom { get; set; }

        [JsonPropertyName("dateUntil")]
        public string DateUntil { get; set; }

    }
}
