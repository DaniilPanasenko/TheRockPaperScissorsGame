using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using TheRockPaperScissorsGame.Client.Clients;
using TheRockPaperScissorsGame.Client.Menu.Library;
using TheRockPaperScissorsGame.Client.Models;

namespace TheRockPaperScissorsGame.Client.Menu
{
    public class AuthorizationMenu : IMenu
    {
        private UserClient _userClient;
        private GameClient _gameClient;
        private StatisticClient _statisticClient;

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
                        await ExecuteLogin();
                        break;
                    case 2:
                        await ExecuteRegistration();
                        break;
                    case 3:
                        return;
                }
            }
        }

        private async Task ExecuteLogin()
        {
            MenuLibrary.Clear();
            while (true)
            {
                MenuLibrary.WriteLineColor("\nLogin\n", ConsoleColor.Yellow);

                string login = MenuLibrary.InputStringValue("login");
                string password = MenuLibrary.InputStringValue("password");

                var response = await _userClient.Login(login, password);

                if (await ResponseHandler(response, "login")) return;
            }
        }

        private async Task ExecuteRegistration()
        {
            MenuLibrary.Clear();
            while (true)
            {
                MenuLibrary.WriteLineColor("\nRegistration\n", ConsoleColor.Yellow);

                string login = MenuLibrary.InputStringValue("login");
                string password = MenuLibrary.InputStringValue("password");

                var response = await _userClient.Registration(login, password);

                if (await ResponseHandler(response, "registration")) return;
            }
        }

        private async Task<bool> ResponseHandler(HttpResponseMessage response, string operation)
        {
            if (response.StatusCode == HttpStatusCode.OK)
            {
                ResponseLibrary.SuccessfullyOperation(operation);

                IMenu menu = new UserMenu(_userClient, _gameClient, _statisticClient);
                await menu.StartAsync();

                return true;
            }
            else if (response.StatusCode == HttpStatusCode.BadRequest
                    || response.StatusCode == HttpStatusCode.Unauthorized)
            {
                await ResponseLibrary.RepeatOperationWithMessageAsync<UserValidaionResponse>(response);
                return false;
            }
            else
            {
                ResponseLibrary.UnknownResponse();
                return true;
            }
        }
    }
}
