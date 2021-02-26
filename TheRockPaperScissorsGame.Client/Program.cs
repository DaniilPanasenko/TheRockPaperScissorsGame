using System;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using TheRockPaperScissorsGame.Client.Clients;
using TheRockPaperScissorsGame.Client.Menu;
using TheRockPaperScissorsGame.Client.Menu.Library;
using TheRockPaperScissorsGame.Client.Models;

namespace TheRockPaperScissorsGame.Client
{
    class Program
    {
        private static UserClient _userClient;
        private static GameClient _gameClient;
        private static StatisticClient _statisticClient;

        private static void Setup()
        {
            var httpClient = new HttpClient();
            var json = File.ReadAllText("clientOptions.json");
            var options = JsonSerializer.Deserialize<ClientOptions>(json);
            httpClient.BaseAddress = new Uri(options.BaseAddress);

            _userClient = new UserClient(httpClient);
            _gameClient = new GameClient(httpClient);
            _statisticClient = new StatisticClient(httpClient);
        }

        private static async Task<int> Main()
        {
            while (true)
            {
                try
                {
                    MenuLibrary.Clear();
                    MenuLibrary.WriteLineColor("The Rock Paper Scissors Game. Designed by Karyna Bilotska and Daniil Panasenko\n", ConsoleColor.DarkGreen);

                    MenuLibrary.PressAnyKey();

                    Setup();

                    IMenu mainMenu = new MainMenu(_userClient, _gameClient, _statisticClient);
                    await mainMenu.StartAsync();

                    return 0;
                }
                catch (Exception)
                {
                    ResponseLibrary.UnknownResponse();
                    MenuLibrary.PressAnyKey();
                }
            }
        }
    }
}