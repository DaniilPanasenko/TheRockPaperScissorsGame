using System;
using System.Text.Json.Serialization;

namespace TheRockPaperScissorsGame.Client.Models
{
    public class ClientOptions
    {
        [JsonPropertyName("baseAddress")]
        public string BaseAddress { get; set; }
    }
}
