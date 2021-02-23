using System;
using System.Net.Http;

namespace TheRockPaperScissorsGame.Client.Clients
{
    public class StatisticClient
    {
        public HttpClient _httpClient;

        public ValuesStorage _valuesStorage;

        public StatisticClient(HttpClient httpClient, ValuesStorage valuesStorage)
        {
            _httpClient = httpClient;
            _valuesStorage = valuesStorage;
        }
    }
}
