using Microsoft.Extensions.Logging;
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

        private readonly ILogger<AuthorizationMenu> _logger;

        private readonly UserMenu _userMenu;

        public AuthorizationMenu(UserClient userClient, UserMenu userMenu,ILogger<AuthorizationMenu> logger)
        {
            _userClient = userClient;
            _userMenu = userMenu;
            _logger = logger;
        }

        public async Task StartAsync()
        {
           _logger.LogInformation("class AuthorizationMenu. StartAsync()");
            while (true)
            {
                _logger.LogInformation("Choosing the Authorization Method");

                MenuLibrary.Clear();

                var options = new string[] { "Login", "Registration", "Back" };
                var command = MenuLibrary.InputMenuItemNumber("Authorization Menu", options);

                _logger.LogInformation("Chose the command");

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
            _logger.LogInformation("class AuthorizationMenu. ExecuteLoginAsync()");

            MenuLibrary.Clear();
            while (true)
            {
                MenuLibrary.WriteLineColor("\nLogin\n", ConsoleColor.Yellow);

                string login = MenuLibrary.InputStringValue("login");
                string password = MenuLibrary.InputStringValue("password");

                _logger.LogInformation("Entered the login and the password");

                var response = await _userClient.LoginAsync(login, password);

                _logger.LogInformation("REQUEST: Sent the login request to the server");

                if (await ResponseHandlerAsync(response, "login"))
                {
                    return;
                }
            }
        }

        private async Task ExecuteRegistrationAsync()
        {
            _logger.LogInformation("class AuthorizationMenu. ExecuteRegistrationAsync()");

            MenuLibrary.Clear();
            while (true)
            {
                MenuLibrary.WriteLineColor("\nRegistration\n", ConsoleColor.Yellow);

                string login = MenuLibrary.InputStringValue("login");
                string password = MenuLibrary.InputStringValue("password");

                _logger.LogInformation("Entered the login and the password");

                var response = await _userClient.RegistrationAsync(login, password);

                _logger.LogInformation("REQUEST: Sent the register request to the server");

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
                _logger.LogInformation("RESPONSE: Successful request (200)");

                ResponseLibrary.SuccessfullyOperation(operation);
                await _userMenu.StartAsync();
                return true;
            }
            else if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                _logger.LogInformation("RESPONSE: Unable to Unauthorized (401)");
                await ResponseLibrary.RepeatOperationWithMessageAsync<string>(response);
                return true;
            }
            else if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                _logger.LogInformation("RESPONSE: Bad request (400)");
                await ResponseLibrary.RepeatOperationWithMessageAsync<UserValidaionResponse>(response);
                return false;
            }
            else
            {
                _logger.LogInformation("RESPONSE: Unknown response");
                throw new HttpListenerException();
            }
        }
    }
}