using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Text.Json;
using System.Threading.Tasks;
using TheRockPaperScissorsGame.Client.Contracts;

namespace TheRockPaperScissorsGame.Client.Clients
{
    public class UserClient
    {
        public HttpClient _httpClient;

        public UserClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<HttpResponseMessage> Login(string login, string password)
        {
            var user = new UserDto(login, password);
            var uri = new Uri(_httpClient.BaseAddress + "users/login");
            var response = await _httpClient.PostAsync(uri, user, new JsonMediaTypeFormatter());
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var token = await response.Content.ReadAsStringAsync();
                token = JsonSerializer.Deserialize<string>(token);
                _httpClient.DefaultRequestHeaders.Add("Token", token);
            }
            return response;
        }

        public async Task<HttpResponseMessage> Registration(string login, string password)
        {
            var user = new UserDto(login, password);
            var uri = new Uri(_httpClient.BaseAddress + "users/register");
            var response = await _httpClient.PostAsync(uri, user, new JsonMediaTypeFormatter());
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var token = await response.Content.ReadAsStringAsync();
                token = JsonSerializer.Deserialize<string>(token);
                _httpClient.DefaultRequestHeaders.Add("Token", token);
            }
            return response;
        }
    }
}
