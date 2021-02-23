using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading.Tasks;
using TheRockPaperScissorsGame.Client.Contracts;
using TheRockPaperScissorsGame.Client.Contracts.Enums;

namespace TheRockPaperScissorsGame.Client.Clients
{
    public class GameClient
    {
        public HttpClient _httpClient;

        public ValuesStorage _valuesStorage;

        public GameClient(HttpClient httpClient, ValuesStorage valuesStorage)
        {
            _httpClient = httpClient;
            _valuesStorage = valuesStorage;
        }

        public async Task<HttpResponseMessage> StartSession(RoomType roomType, string roomId)
        {
            var options = new GameOptionsDTO(roomType, roomId);
            var uri = new Uri(_httpClient.BaseAddress + "start_session");
            var response = await _httpClient.PostAsync(uri, options, new JsonMediaTypeFormatter());
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var room = await response.Content.ReadAsStringAsync();
                _valuesStorage.RoomId = room;
            }
            return response;
        }

        public async Task<HttpResponseMessage> CheckSession()
        {
            var uri = new Uri(_httpClient.BaseAddress + "check_session/" + _valuesStorage.RoomId);
            var response = await _httpClient.GetAsync(uri);
            return response;
        }

        public async Task<HttpResponseMessage> DoMove(Move move)
        {
            var uri = new Uri(_httpClient.BaseAddress + "do_move/" + _valuesStorage.RoomId);
            var response = await _httpClient.PostAsync(uri, move, new JsonMediaTypeFormatter());
            return response;
        }

        public async Task<HttpResponseMessage> FinishSession()
        {
            var uri = new Uri(_httpClient.BaseAddress +"finish_session/" + _valuesStorage.RoomId);
            var response = await _httpClient.PostAsync(uri, null);
            return response;
        }
    }
}
