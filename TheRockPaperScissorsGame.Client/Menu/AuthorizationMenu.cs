using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using TheRockPaperScissorsGame.Client.Clients;
using TheRockPaperScissorsGame.Client.Models;
using TheRockPaperScissorsGame.Client.Services;

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
                Console.Clear();
                var options = new string[] { "Login", "Registration", "Back" };
                var command = MenuLibrary.InputMenuItemNumber("Authorization", options);
                switch (command)
                {
                    case 1:
                        await ExecuteLogin();
                        break;
                    case 2:
                        await ExecuteRegistration();
                        break;
                    case 3:
                    default:
                        return;
                }
            }
        }

        private async Task ExecuteLogin()
        {
            Console.Clear();
            while (true)
            {
                MenuLibrary.WriteLineColor("\nLogin\n", ConsoleColor.Yellow);
                string login = MenuLibrary.InputStringValue("login");
                string password = MenuLibrary.InputStringValue("password");

                var response = await _userClient.Login(login, password);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    MenuLibrary.WriteLineColor("\nSuccessfully login\n", ConsoleColor.Green);
                    Thread.Sleep(1000);
                    IMenu menu = new UserMenu(_userClient, _gameClient, _statisticClient);
                    await menu.StartAsync();
                    return;
                }
                else if (response.StatusCode == HttpStatusCode.BadRequest)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    var textResponse = ResponseDeserializer.Deserialize<UserValidaionResponse>(jsonResponse);
                    MenuLibrary.WriteLineColor($"\n{textResponse}", ConsoleColor.Red);
                    Console.WriteLine("Please repeat login.");
                }
                else if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    var textResponse = ResponseDeserializer.Deserialize<string>(jsonResponse);
                    MenuLibrary.WriteLineColor($"\n{textResponse}\n", ConsoleColor.Red);
                    Console.WriteLine("Please repeat login.");
                }
                else
                {
                    MenuLibrary.WriteLineColor("\nSorry, something went wrong, try it later\n", ConsoleColor.Red);
                    Thread.Sleep(3000);
                    return;
                }
            }
        }

        private async Task ExecuteRegistration()
        {
            Console.Clear();
            while (true)
            {
                MenuLibrary.WriteLineColor("\nRegistration\n", ConsoleColor.Yellow);
                string login = MenuLibrary.InputStringValue("login");
                string password = MenuLibrary.InputStringValue("password");

                var response = await _userClient.Registration(login, password);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    MenuLibrary.WriteLineColor("\nSuccessfully registration\n", ConsoleColor.Green);
                    Thread.Sleep(1000);
                    IMenu menu = new UserMenu(_userClient, _gameClient, _statisticClient);
                    await menu.StartAsync();
                    return;
                }
                else if (response.StatusCode == HttpStatusCode.BadRequest)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    var textResponse = ResponseDeserializer.Deserialize<UserValidaionResponse>(jsonResponse);
                    MenuLibrary.WriteLineColor($"\n{textResponse}", ConsoleColor.Red);
                    Console.WriteLine("Please repeat registration.");
                }
                else if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    var textResponse = ResponseDeserializer.Deserialize<string>(jsonResponse);
                    MenuLibrary.WriteLineColor($"\n{textResponse}\n", ConsoleColor.Red);
                    Console.WriteLine("Please repeat registartion.");
                }
                else
                {
                    MenuLibrary.WriteLineColor("\nSorry, something went wrong, try it later\n", ConsoleColor.Red);
                    Thread.Sleep(3000);
                    return;
                }
            }
        }
    }
}
