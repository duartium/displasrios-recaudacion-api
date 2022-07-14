using System.Text.Json.Serialization;

namespace Displasrios.Recaudacion.Core.Models
{
    public class ProviderCreate
    {
        [JsonPropertyName("ruc")]
        public string Ruc { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("address")]
        public string Address { get; set; }

        [JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonPropertyName("phone")]
        public string Phone { get; set; }

        [JsonPropertyName("user_creation")]
        public string UserCreation { get; set; }
    }

    public class ProviderUpdate : ProviderCreate {
        [JsonPropertyName("id")]
        public int Id { get; set; }
    }

}
