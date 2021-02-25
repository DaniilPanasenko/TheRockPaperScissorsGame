using System.Text.Json.Serialization;

namespace TheRockPaperScissorsGame.Client.Contracts
{
    public class MovesDto
    {
        [JsonPropertyName("rockCount")]
        public int RockCount { get; set; }

        [JsonPropertyName("scissorsCount")]
        public int ScissorsCount { get; set; }

        [JsonPropertyName("paperCount")]
        public int PaperCount { get; set; }
    }
}
