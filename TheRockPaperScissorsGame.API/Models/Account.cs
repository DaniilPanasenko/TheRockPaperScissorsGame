using System.ComponentModel.DataAnnotations;

namespace TheRockPaperScissorsGame.API.Models
{
    public class Account
    {
        public Account()
        {
        }

        [Required]
        [StringLength(maximumLength: 20, MinimumLength = 8)]
        public string Login { get; set; }

        [Required]
        [StringLength(maximumLength: 20, MinimumLength = 6)]
        public string Password { get; set; }
    }
}
