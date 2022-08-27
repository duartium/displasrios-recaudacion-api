using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Displasrios.Recaudacion.Core.DTOs.Sales
{
    public class SaleBySeller
    {
        [JsonPropertyName("description")]
        public string Description { get; set; }
    }
}