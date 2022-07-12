using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Displasrios.Recaudacion.Core.DTOs
{
    public class ItemCatalogueDto
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }
    }

    public class CatalogueDto
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("items")]
        public List<ItemCatalogueDto> Catalogue { get; set; }
    }

}
