using System;
using System.Text.Json;

namespace TheRockPaperScissorsGame.Client.Services
{
    public static class ResponseDeserializer
    {
        public static T Deserialize<T>(string json)
        {
            return JsonSerializer.Deserialize<T>(json);
        }
    }
}
