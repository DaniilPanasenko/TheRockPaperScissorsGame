using System.Text.Json.Serialization;

namespace TheRockPaperScissorsGame.Client.Contracts
{
    public class UserResultDto<T>
    {
        public UserResultDto()
        {
        }

        [JsonPropertyName("login")]
        public string Login { get; set; }

        [JsonPropertyName("result")]
        public T Result { get; set; }
    }
}
