using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarDealer.Models;
using Newtonsoft.Json;

namespace CarDealer.DTOs.Import
{
    public class ImportCarsdto
    {
        [JsonProperty("make")]
        public string Make { get; set; } = null!;

        [JsonProperty("model")]
        public string Model { get; set; } = null!;

        [JsonProperty("traveledDistance")]
        public long TraveledDistance { get; set; }


        [JsonProperty("partsId")]
        public int[] Parts { get; set; } = null!;
    }


}
