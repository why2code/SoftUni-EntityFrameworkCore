using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Boardgames.DataProcessor.ImportDto
{
    public class ImportSellerDto
    {
        [Required]
        [MinLength(5)]
        [MaxLength(20)]
        [JsonProperty("Name")]
        public string Name { get; set; } = null!;


        [Required]
        [MinLength(2)]
        [MaxLength(30)]
        [JsonProperty("Address")]
        public string Address { get; set; } = null!;

        [Required]
        [JsonProperty("Country")]
        public string Country  { get; set; } = null!;

        [Required]
        [RegularExpression("^[www]{3}\\.{1}[A-Za-z\\d\\-]+\\.com{1}$")]
        [JsonProperty("Website")]
        public string Website  { get; set; } = null!;


        [JsonProperty("Boardgames")]
        public int[] BoardgamesSellers { get; set; } 
    }
}
