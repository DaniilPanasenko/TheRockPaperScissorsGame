using System.Text.Json.Serialization;

namespace TheRockPaperScissorsGame.Client.Contracts
{
    public class ResultsDto
    {
        public ResultsDto()
        {
        }

        [JsonPropertyName("winCount")]
        public int WinCount { get; set; }

        [JsonPropertyName("drawCount")]
        public int DrawCount { get; set; }

        [JsonPropertyName("lossCount")]
        public int LossCount { get; set; }
    }
}
