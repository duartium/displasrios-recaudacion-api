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
        public decimal Price { get; set; }

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
        public decimal CustomerPayment { get; set; }

        [JsonPropertyName("change")]
        public decimal Change { get; set; }

        [JsonPropertyName("discount")]
        public decimal Discount { get; set; }

        [JsonPropertyName("subtotal")]
        public decimal Subtotal { get; set; }

        [JsonPropertyName("iva")]
        public decimal Iva { get; set; }

        [JsonPropertyName("total")]
        public decimal Total { get; set; }

        [JsonIgnore]
        public int IdUser { get; set; }

        [JsonIgnore]
        public string Username { get; set; }
    }

    public class OrderSummaryDto
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("order_number")]
        public string OrderNumber { get; set; }

        [JsonPropertyName("full_names")]
        public string FullNames { get; set; }

        [JsonPropertyName("total_amount")]
        public string TotalAmount { get; set; }

        [JsonPropertyName("date")]
        public string Date { get; set; }
    }

}
