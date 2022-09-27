using System.Text.Json.Serialization;

namespace Displasrios.Recaudacion.Core.DTOs
{
    public class SummaryOrdersOfDay
    {
        [JsonPropertyName("id_order")]
        public int IdOrder { get; set; }

        [JsonPropertyName("order_number")]
        public string OrderNumber { get; set; }

        [JsonPropertyName("full_names")]
        public string FullNames { get; set; }

        [JsonPropertyName("date")]
        public string Date { get; set; }

        [JsonPropertyName("stage")]
        public string Stage { get; set; }

        [JsonPropertyName("total_amount")]
        public decimal TotalAmount { get; set; }

        [JsonPropertyName("username")]
        public string Username { get; set; }
    }
}
