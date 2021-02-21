using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TheRockPaperScissorsGame.API.Models
{
    public class Account
    {
        public Account()
        {
        }

        [Required]
        [StringLength(maximumLength: 20, MinimumLength = 8)]
        [JsonPropertyName("login")]
        public string Login { get; set; }

        [Required]
        [StringLength(maximumLength: 20, MinimumLength = 6)]
        [JsonPropertyName("password")]
        public string Password { get; set; }
    }
}
