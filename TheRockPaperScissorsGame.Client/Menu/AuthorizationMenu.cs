using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using TheRockPaperScissorsGame.Client.Clients;
using TheRockPaperScissorsGame.Client.Menu.Library;
using TheRockPaperScissorsGame.Client.Models;

namespace TheRockPaperScissorsGame.Client.Menu
{
    public class AuthorizationMenu : IMenu
    {
        private readonly UserClient _userClient;
        private readonly GameClient _gameClient;
        private readonly StatisticClient _statisticClient;

        public AuthorizationMenu(UserClient userClient, GameClient gameClient, StatisticClient statisticClient)
        {
            _userClient = userClient;
            _gameClient = gameClient;
            _statisticClient = statisticClient;
        }

        public async Task StartAsync()
        {
            while (true)
            {
                MenuLibrary.Clear();

                var options = new string[] { "Login", "Registration", "Back" };
                var command = MenuLibrary.InputMenuItemNumber("Authorization Menu", options);

                switch (command)
                {
                    case 1:
                        await ExecuteLoginAsync();
                        break;
                    case 2:
                        await ExecuteRegistrationAsync();
                        break;
                    case 3:
                        return;
                }
            }
        }

        private async Task ExecuteLoginAsync()
        {
            MenuLibrary.Clear();
            while (true)
            {
                MenuLibrary.WriteLineColor("\nLogin\n", ConsoleColor.Yellow);

                string login = MenuLibrary.InputStringValue("login");
                string password = MenuLibrary.InputStringValue("password");

                var response = await _userClient.LoginAsync(login, password);

                if (await ResponseHandlerAsync(response, "login"))
                {
                    return;
                }
            }
        }

        private async Task ExecuteRegistrationAsync()
        {
            MenuLibrary.Clear();
            while (true)
            {
                MenuLibrary.WriteLineColor("\nRegistration\n", ConsoleColor.Yellow);

                string login = MenuLibrary.InputStringValue("login");
                string password = MenuLibrary.InputStringValue("password");

                var response = await _userClient.RegistrationAsync(login, password);

                if (await ResponseHandlerAsync(response, "registration"))
                {
                    return;
                }
            }
        }

        private async Task<bool> ResponseHandlerAsync(HttpResponseMessage response, string operation)
        {
            if (response.StatusCode == HttpStatusCode.OK)
            {
                ResponseLibrary.SuccessfullyOperation(operation);

                IMenu menu = new UserMenu(_gameClient, _statisticClient);
                await menu.StartAsync();
                return true;
            }
            else if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                await ResponseLibrary.RepeatOperationWithMessageAsync<string>(response);
                return true;
            }
            else if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                await ResponseLibrary.RepeatOperationWithMessageAsync<UserValidaionResponse>(response);
                return false;
            }
            else
            {
                throw new HttpListenerException();
            }
        }
    }
}