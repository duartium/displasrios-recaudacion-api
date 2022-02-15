using System.Text.Json.Serialization;

namespace Displasrios.Recaudacion.Core.DTOs
{
    public class UserDto
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("username")]
        public string Username { get; set; }

        [JsonPropertyName("created_at")]
        public string CreatedAt { get; set; }
    }
}
