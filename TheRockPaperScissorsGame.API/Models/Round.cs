using System.Text.Json.Serialization;
using TheRockPaperScissorsGame.API.Enums;

namespace TheRockPaperScissorsGame.API.Models
{
    public class Round
    {
        public Round()
        {
        }

        [JsonPropertyName("player1Move")]
        public Move? Player1Move { get; set; }

        [JsonPropertyName("player2Move")]
        public Move? Player2Move { get; set; }

        [JsonIgnore]
        public WinType WinType
        {
            get
            {
                if (Player1Move == Move.Rock && Player2Move == Move.Rock ||
                     Player1Move == Move.Paper && Player2Move == Move.Paper ||
                     Player1Move == Move.Scissors && Player2Move == Move.Scissors)
                    return WinType.Draw;
                else
                if (Player1Move == Move.Rock && Player2Move == Move.Paper ||
                     Player1Move == Move.Paper && Player2Move == Move.Scissors ||
                     Player1Move == Move.Scissors && Player2Move == Move.Rock)
                    return WinType.SecondPlayer;
                else
                    return WinType.FirstPlayer;
            }
        }
    }
}
