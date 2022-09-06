using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Displasrios.Recaudacion.Core.DTOs
{
    public class CustomerDebtDto
    {
        [JsonPropertyName("totalDebts")]
        public decimal TotalDebts { get; set; }

        [JsonPropertyName("lastVisit")]
        public string LastVisit { get; set; }
        
        [JsonPropertyName("fullnames")]
        public string Fullnames { get; set; }

        [JsonPropertyName("ordersReceivable")]
        public List<OrderSummaryDto> OrdersReceivable{ get; set; }
    }
}
