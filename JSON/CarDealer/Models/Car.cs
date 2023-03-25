using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace CarDealer.Models
{
    public class Car
    {
        public Car()
        {
            this.Sales = new HashSet<Sale>();
            this.PartsCars = new HashSet<PartCar>();
        }
        public int Id { get; set; }

        public string Make { get; set; } = null!;

        public string Model { get; set; } = null!;

        public long TraveledDistance { get; set; }

        public virtual ICollection<Sale> Sales { get; set; } = null!;

        [JsonIgnore]
        public virtual ICollection<PartCar> PartsCars { get; set; } = null!;

        
        //[NotMapped]
        //public decimal Price => this.PartsCars.Select(pc => pc.Part.Price).Sum();
    }
}
