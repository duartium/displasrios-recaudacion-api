using Newtonsoft.Json;

namespace Displasrios.Recaudacion.Core.DTOs
{
    public class UserDto
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("username")]
        public string Username { get; set; }

        [JsonProperty("created_at")]
        public string CreatedAt { get; set; }
    }
}
