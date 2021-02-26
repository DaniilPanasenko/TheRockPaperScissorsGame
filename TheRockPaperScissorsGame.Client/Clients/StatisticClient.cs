using System;
using System.Net.Http;
using System.Threading.Tasks;
using TheRockPaperScissorsGame.Client.Contracts.Enums;

namespace TheRockPaperScissorsGame.Client.Clients
{
    public class StatisticClient
    {
        public readonly HttpClient _httpClient;

        public StatisticClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<HttpResponseMessage> GetLeaderboardAsync(int? amount, StatisticsType statisticsType)
        {
            var uri = new Uri(_httpClient.BaseAddress + $"statistics/leaderboard?amount={amount}&type={statisticsType}");
            var response = await _httpClient.GetAsync(uri);
            return response;
        }

        public async Task<HttpResponseMessage> GetUserResultsAsync()
        {
            var uri = new Uri(_httpClient.BaseAddress + $"statistics/user_results");
            var response = await _httpClient.GetAsync(uri);
            return response;
        }

        public async Task<HttpResponseMessage> GetUserGameTimeAsync()
        {
            var uri = new Uri(_httpClient.BaseAddress + $"statistics/user_time");
            var response = await _httpClient.GetAsync(uri);
            return response;
        }

        public async Task<HttpResponseMessage> GetUserMovesAsync()
        {
            var uri = new Uri(_httpClient.BaseAddress + $"statistics/user_moves");
            var response = await _httpClient.GetAsync(uri);
            return response;
        }

        public async Task<HttpResponseMessage> GetUserResultByIntervalAsync(int? amount, TimeInterval interval)
<<<<<<< HEAD
        {
            var uri = new Uri(_httpClient.BaseAddress + $"statistics/result_by_time?amount={amount}&type={interval}");
=======
        {
            var uri = new Uri(_httpClient.BaseAddress + $"statistics/results_by_time?amount={amount}&timeInterval={(int)interval}");
>>>>>>> 8f15b79f4f62d77b3948675ba70f3eabb0843599
            var response = await _httpClient.GetAsync(uri);
            return response;
        }
    }
}
