using System.Text.Json.Serialization;

namespace Displasrios.Recaudacion.Core.DTOs
{
    public class CollectorResumeDto
    {
        [JsonPropertyName("id")]
        public int IdUser { get; set; }

        [JsonPropertyName("description")]
        public string FullUsername { get; set; }
    }
}
