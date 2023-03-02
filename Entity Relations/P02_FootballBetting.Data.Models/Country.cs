
using System.ComponentModel.DataAnnotations;
using P02_FootballBetting.Data.Common;

namespace P02_FootballBetting.Data.Models
{
    public class Country
    {
        public Country()
        {
            this.Towns = new HashSet<Town>();
        }
        public int CountryId { get; set; }

        [MaxLength(CommonConfigs.CountryNameLength)]
        public string Name { get; set; } = null!;

        public virtual ICollection<Town> Towns { get; set; } = null!;
    }
}
