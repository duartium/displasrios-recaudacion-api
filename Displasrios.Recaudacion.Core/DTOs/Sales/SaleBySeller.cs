using System.Text.Json.Serialization;

namespace Displasrios.Recaudacion.Core.DTOs.Sales
{
    public class IncomeBySellersDto
    {
        [JsonPropertyName("total")]
        public string Total { get; set; }

        [JsonPropertyName("user")]
        public string User { get; set; }
    }

    public class SellerPersonalReportDto
    {
        [JsonPropertyName("totalSalesToday")]
        public decimal TotalSalesToday { get; set; }

        [JsonPropertyName("totalMonthSales")]
        public decimal totalMonthSales { get; set; }

        [JsonPropertyName("numOrdersReceivable")]
        public int NumOrdersReceivable { get; set; }
    }
}