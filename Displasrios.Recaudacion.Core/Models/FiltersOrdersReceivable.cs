using System.Text.Json.Serialization;

namespace Displasrios.Recaudacion.Core.Models
{
    public class FiltersOrdersReceivable
    {
        [JsonPropertyName("search_type")]
        public string SearchType { get; set; }

        [JsonPropertyName("id_user")]
        public int IdUser { get; set; }

        [JsonPropertyName("identification")]
        public string Identification { get; set; }

        [JsonPropertyName("names")]
        public string Names { get; set; }

        [JsonPropertyName("order_number")]
        public string OrderNumber { get; set; }

        [JsonPropertyName("date_from")]
        public string DateFrom { get; set; }

        [JsonPropertyName("date_until")]
        public string DateUntil { get; set; }
    }
}
