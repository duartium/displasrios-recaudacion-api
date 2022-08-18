using System.Text.Json.Serialization;

namespace Displasrios.Recaudacion.Core.Models
{
    public class VisitCreation
    {
        [JsonPropertyName("id_order")]
        public int OrderId { get; set; }

        [JsonIgnore]
        public string Username { get; set; }

        [JsonPropertyName("observations")]
        public string Observations { get; set; }
    }
}
