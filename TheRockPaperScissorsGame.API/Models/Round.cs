using System.Text.Json.Serialization;
using TheRockPaperScissorsGame.API.Enums;

namespace TheRockPaperScissorsGame.API.Models
{
    public class Round
    {
        public Round()
        {
        }

        //public Round(Move?)
        //{

        //}

        [JsonPropertyName("player1Move")]
        public Move? Player1Move { get; set; }

        [JsonPropertyName("player2Move")]
        public Move? Player2Move { get; set; }
    }
}
