using Displasrios.Recaudacion.Core.Models.Security;
using System.Text.Json.Serialization;

namespace Displasrios.Recaudacion.Core.Models
{
    public class CustomerUpdate : CustomerCreate
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
    }

    public class CustomerCreate : Audit
    {
        [JsonPropertyName("identification")]
        public string Identification { get; set; }

        [JsonPropertyName("names")]
        public string Names { get; set; }

        [JsonPropertyName("surnames")]
        public string Surnames { get; set; }

        [JsonPropertyName("address")]
        public string Address { get; set; }

        [JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonPropertyName("phone")]
        public string Phone { get; set; }
    }
}
