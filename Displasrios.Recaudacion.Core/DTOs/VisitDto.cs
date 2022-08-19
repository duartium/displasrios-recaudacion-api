using System.Text.Json.Serialization;

namespace Displasrios.Recaudacion.Core.DTOs
{
    public class VisitDto
    {
        [JsonPropertyName("username")]
        public string Username { get; set; }

        [JsonPropertyName("observations")]
        public string Observations { get; set; }

        [JsonPropertyName("date")]
        public string Date { get; set; }
    }
}
