using TheRockPaperScissorsGame.API.Enums;

namespace TheRockPaperScissorsGame.API.Contracts
{
    public class RoundResultDto
    {
        public string OpponentMove { get; set; }

        public RoundResultFromUserSide Result { get; set; }
    }
}
