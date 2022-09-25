using System.Text.Json.Serialization;

namespace Displasrios.Recaudacion.Core.DTOs
{
    public class CustomerDto
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("type")]
        public int Type { get; set; }

        [JsonPropertyName("ident_type")]
        public string IdentType { get; set; }

        [JsonPropertyName("identification")]
        public string Identification { get; set; }

        [JsonPropertyName("names")]
        public string Names { get; set; }

        [JsonPropertyName("surnames")]
        public string Surnames { get; set; }

        [JsonPropertyName("address")]
        public string Address { get; set; }

        [JsonPropertyName("phone_number")]
        public string PhoneNumber { get; set; }

        [JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonPropertyName("created_at")]
        public string CreatedAt { get; set; }
    }

    public class BestCustomerDto
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("totalPurchases")]
        public decimal TotalPurchases { get; set; }

        [JsonPropertyName("fullNames")]
        public string FullNames { get; set; }
    }
}
