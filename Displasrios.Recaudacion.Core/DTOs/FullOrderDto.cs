using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Displasrios.Recaudacion.Core.DTOs
{
    public class Item
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("price")]
        public string Price { get; set; }

        [JsonPropertyName("quantity")]
        public int Quantity { get; set; }
    }

    public class FullOrderDto
    {
        [JsonPropertyName("id_client")]
        public int IdClient { get; set; }

        [JsonPropertyName("items")]
        public List<Item> Items { get; set; }

        [JsonPropertyName("payment_method")]
        public int PaymentMethod { get; set; }

        [JsonPropertyName("payment_mode")]
        public int PaymentMode { get; set; }

        [JsonPropertyName("customer_payment")]
        public string CustomerPayment { get; set; }

        [JsonPropertyName("change")]
        public string Change { get; set; }

        [JsonPropertyName("subtotal")]
        public string Subtotal { get; set; }

        [JsonPropertyName("iva")]
        public string Iva { get; set; }

        [JsonPropertyName("total")]
        public string Total { get; set; }

        [JsonIgnore]
        public int IdUser { get; set; }

        [JsonIgnore]
        public string Username { get; set; }
    }

}
