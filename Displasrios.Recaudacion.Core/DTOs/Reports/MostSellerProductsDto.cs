using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Displasrios.Recaudacion.Core.DTOs.Reports
{
    public class MostSelledProductDto
    {
        [Key]
        [JsonPropertyName("id")]
        public int IdProducto { get; set; }

        [JsonPropertyName("soldUnits")]
        public int UnidadesVendidas { get; set; }

        [JsonPropertyName("name")]
        public string Nombre { get; set; }
    }
}