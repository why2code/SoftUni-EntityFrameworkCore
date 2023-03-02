

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using P02_FootballBetting.Data.Common;

namespace P02_FootballBetting.Data.Models
{
    public class User
    {
        public User()
        {
            this.Bets = new HashSet<Bet>();
        }
        public int UserId { get; set; }

        [MaxLength(CommonConfigs.UserNameMaxLength)]
        public string Username { get; set; } = null!;


        [PasswordPropertyText] 
        [MaxLength(CommonConfigs.PasswordMaxLength)]
        public string Password { get; set; } = null!;


        [MaxLength(CommonConfigs.EmailMaxLength)]
        public string Email { get; set; } = null!;

        [MaxLength(CommonConfigs.UserFullNameMaxLength)]
        public string Name { get; set; } = null!;

        public decimal Balance { get; set; }

        public virtual ICollection<Bet> Bets { get; set; } = null!;

    }
}
