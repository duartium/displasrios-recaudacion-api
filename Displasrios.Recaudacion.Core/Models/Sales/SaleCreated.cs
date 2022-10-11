using System.Text.Json.Serialization;

namespace Displasrios.Recaudacion.Core.Models.Sales
{
    public class SaleCreated
    {
        [JsonPropertyName("orderNumber")]
        public int OrderNumber { get; set; }

        [JsonPropertyName("sendMail")]
        public bool SendEmail { get; set; }
    }
}
