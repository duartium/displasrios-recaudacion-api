using System.Text.Json.Serialization;

namespace Displasrios.Recaudacion.Core.DTOs
{
    public class ProductDto
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("code")]
        public string Code { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("cost")]
        public string Cost { get; set; }

        [JsonPropertyName("sale_price")]
        public string SalePrice { get; set; }

        [JsonPropertyName("stock")]
        public int Stock { get; set; }

        [JsonPropertyName("quantity_package")]
        public int QuantityPackage { get; set; }

        [JsonPropertyName("quantity_lump")]
        public int QuantityLump { get; set; }

        [JsonPropertyName("discount")]
        public string Discount { get; set; }

        [JsonPropertyName("tarifa_id")]
        public int IvaTariff { get; set; }

        [JsonPropertyName("category_id")]
        public int CategoryId { get; set; }

        [JsonPropertyName("category_name")]
        public string CategorName { get; set; }

        [JsonPropertyName("provider_id")]
        public int ProdiverId { get; set; }

        [JsonPropertyName("provider_name")]
        public string ProdiverName { get; set; }
    }

    public class ProductSaleDto
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("code")]
        public string Code { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("stock")]
        public int Stock { get; set; }

        [JsonPropertyName("sale_price")]
        public string SalePrice { get; set; }
    }

    public class ProductResumeDto
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("price")]
        public decimal Price { get; set; }

        [JsonPropertyName("quantity")]
        public int Quantity { get; set; }

        [JsonPropertyName("total")]
        public decimal Total { get; set; }
    }

    public class ProductCreation
    {
        [JsonPropertyName("code")]
        public string Code { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("cost")]
        public string Cost { get; set; }

        [JsonPropertyName("sale_price")]
        public string SalePrice { get; set; }

        [JsonPropertyName("stock")]
        public int Stock { get; set; }

        [JsonPropertyName("quantity_package")]
        public int QuantityPackage { get; set; }

        [JsonPropertyName("quantity_lump")]
        public int QuantityLump { get; set; }

        [JsonPropertyName("discount")]
        public string Discount { get; set; }

        [JsonPropertyName("tarifa_id")]
        public int IvaTariff { get; set; }

        [JsonPropertyName("category_id")]
        public int CategoryId { get; set; }

        [JsonPropertyName("provider_id")]
        public int ProviderId { get; set; }

        [JsonPropertyName("user_creation")]
        public string UserCreation { get; set; }
    }

}
