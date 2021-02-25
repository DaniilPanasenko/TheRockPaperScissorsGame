using System;
using System.Text.Json.Serialization;

namespace TheRockPaperScissorsGame.Client.Contracts
{
    public class RoundResultDto
    {
        [JsonPropertyName("opponentMove")]
        public string OpponentMove { get; set; }

        [JsonPropertyName("result")]
        public int Result { get; set; }
    }
}
