using System.Text.Json.Serialization;

namespace Displasrios.Recaudacion.Core.DTOs
{
    public class CustomerSearchOrderDto
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("full_names")]
        public string FullNames { get; set; }

        [JsonPropertyName("identification")]
        public string Identification { get; set; }
    }
}
