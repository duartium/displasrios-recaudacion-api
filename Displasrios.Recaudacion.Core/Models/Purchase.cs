using Displasrios.Recaudacion.Core.DTOs;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Displasrios.Recaudacion.Core.Models
{
    public class PurchaseCreate
    {
        [JsonPropertyName("idProvider")]
        public int IdProvider { get; set; }

        [JsonPropertyName("date")]
        public string Date { get; set; }

        [JsonPropertyName("numInvoice")]
        public string NumInvoice { get; set; }

        [JsonPropertyName("items")]
        public List<Item> Items { get; set; }

        [JsonPropertyName("total")]
        public decimal Total { get; set; }

        [JsonPropertyName("observations")]
        public string Observations { get; set; }

        [JsonIgnore]
        public string UserCreation { get; set; }
    }
}