using System.ComponentModel.DataAnnotations;
using P02_FootballBetting.Data.Common;

namespace P02_FootballBetting.Data.Models
{
    public class Position
    {

        public Position()
        {
            this.Players = new HashSet<Player>();
        }
        public int PositionId { get; set; }

        [MaxLength(CommonConfigs.PositionMaxLength)]
        public string Name { get; set; } = null!;

        public virtual ICollection<Player> Players { get; set; } = null!;
    }
}
