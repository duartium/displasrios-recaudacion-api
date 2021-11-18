using System.Text.Json.Serialization;

namespace Displasrios.Recaudacion.Core.Models
{
    public class UserLogin
    {
        [JsonPropertyName("username")]
        public string Username { get; set; }

        [JsonPropertyName("password")]
        public string Password { get; set; }
    }
}
