using System.Text.Json.Serialization;

namespace TheRockPaperScissorsGame.Client.Contracts
{
    public class ResultByTimeDto
    {
        [JsonPropertyName("time")]
        public string Time { get; set; }

        [JsonPropertyName("winCount")]
        public int WinCount { get; set; }

        [JsonPropertyName("lossCount")]
        public int LossCount { get; set; }
    }
}
