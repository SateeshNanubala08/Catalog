using System;  
using System.Text.Json.Serialization;

namespace Catalog.Models
{
    public class Catalog
    {
        [JsonPropertyName("Cagtid")]

        public int CatId { get; set; }

        [JsonPropertyName("CatName")]

        public string CatName { get; set; }

        [JsonPropertyName("CDate")]

        public DateTime CDate { get; set; }

        [JsonPropertyName("Price")]

        public double Price { get; set; }

        [JsonPropertyName("Remarks")]

        public string Remarks { get; set; }
    }
}
