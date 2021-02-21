using System.Collections.Generic;
using System.Text.Json.Serialization;
using TheRockPaperScissorsGame.API.Enums;

namespace TheRockPaperScissorsGame.API.Models
{
    public class Session
    {
        public Session()
        {
        }

        [JsonPropertyName("roomNumber")]
        public string RoomNumber { get; set; }

        [JsonPropertyName("roomType")]
        public RoomType RoomType { get; set; }

        [JsonPropertyName("rounds")]
        public List<Round> Rounds { get; set; }
    }
}
