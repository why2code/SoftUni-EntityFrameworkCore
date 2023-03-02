using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using P02_FootballBetting.Data.Common;

namespace P02_FootballBetting.Data.Models
{
    public class Town
    {
        public Town()
        {
            this.Teams = new HashSet<Team>();
            this.Players = new HashSet<Player>();
        }
        public int TownId { get; set; }

        [MaxLength(CommonConfigs.TownNameLength)]
        public string Name { get; set; } = null!;


        [ForeignKey(nameof(Country))]
        public int CountryId { get; set; }
        public Country Country { get; set; }


        public virtual ICollection<Team> Teams { get; set; } = null!;

        public virtual ICollection<Player> Players { get; set; } = null!;

    }
}
