using System.Text.Json.Serialization;

namespace Displasrios.Recaudacion.Core.Models.Sales
{
    public class IncomeBySellers
    {
        [JsonPropertyName("dateFrom")]
        public string DateFrom { get; set; }

        [JsonPropertyName("dateUntil")]
        public string DateUntil { get; set; }

    }

    public class SalesSellerToday
    {
        [JsonPropertyName("idUser")]
        public int IdUser { get; set; }

        [JsonPropertyName("amount")]
        public decimal Amount { get; set; }

        [JsonIgnore]
        public string Username { get; set; }
    }

}
