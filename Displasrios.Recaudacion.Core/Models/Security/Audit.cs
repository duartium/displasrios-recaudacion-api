using System.Text.Json.Serialization;

namespace Displasrios.Recaudacion.Core.Models.Security
{
    public class Audit
    {
        [JsonIgnore]
        public string CurrentUser { get; set; }
    }
}
