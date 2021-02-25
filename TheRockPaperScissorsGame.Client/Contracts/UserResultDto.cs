using System.Text.Json.Serialization;

namespace TheRockPaperScissorsGame.Client.Contracts
{
    public class UserResultDto
    {
        public UserResultDto()
        {
        }

        [JsonPropertyName("login")]
        public string Login { get; set; }

        [JsonPropertyName("result")]
        public string Result { get; set; }
    }
}
