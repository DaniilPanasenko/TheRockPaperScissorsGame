using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Text.Json;
using System.Threading.Tasks;
using TheRockPaperScissorsGame.Client.Contracts;
using TheRockPaperScissorsGame.Client.Contracts.Enums;

namespace TheRockPaperScissorsGame.Client.Clients
{
    public class GameClient
    {
        private readonly HttpClient _httpClient;

        private string _roomId;

        public GameClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<HttpResponseMessage> StartSessionAsync(RoomType roomType, string roomId)
        {
            var options = new GameOptionsDto(roomType, roomId);
            var uri = new Uri(_httpClient.BaseAddress + "game/start_session");
            var response = await _httpClient.PostAsync(uri, options, new JsonMediaTypeFormatter());

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var room = await response.Content.ReadAsStringAsync();
                room = JsonSerializer.Deserialize<string>(room);
                _roomId = room;
            }

            return response;
        }

        public async Task<HttpResponseMessage> CheckSessionAsync()
        {
            var uri = new Uri(_httpClient.BaseAddress + "game/check_session/" + _roomId);
            var response = await _httpClient.GetAsync(uri);
            return response;
        }

        public async Task<HttpResponseMessage> DoMoveAsync(Move move)
        {
            var uri = new Uri(_httpClient.BaseAddress + "game/do_move/" + _roomId);
            var response = await _httpClient.PostAsync(uri, move, new JsonMediaTypeFormatter());
            return response;
        }

        public async Task<HttpResponseMessage> CheckMoveAsync()
        {
            var uri = new Uri(_httpClient.BaseAddress + "game/check_move/" + _roomId);
            var response = await _httpClient.GetAsync(uri);
            return response;
        }

        public async Task<HttpResponseMessage> FinishSessionAsync()
        {
            var uri = new Uri(_httpClient.BaseAddress + "game/finish_session/" + _roomId);
            var response = await _httpClient.PostAsync(uri, null);
            return response;
        }
    }
}