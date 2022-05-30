using System.Text.Json.Serialization;

namespace Displasrios.Recaudacion.Core.Models
{
    public class UserCreation
    {
        [JsonPropertyName("identification")]
        public string Identification { get; set; }

        [JsonPropertyName("names")]
        public string Names { get; set; }

        [JsonPropertyName("surnames")]
        public string Surnames { get; set; }

        [JsonPropertyName("type")]
        public int Type { get; set; }

        [JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonIgnore]
        public string Username { get; set; }

        [JsonIgnore]
        public string Password { get; set; }

        [JsonIgnore]
        public string CurrentUser { get; set; }
    }
}
