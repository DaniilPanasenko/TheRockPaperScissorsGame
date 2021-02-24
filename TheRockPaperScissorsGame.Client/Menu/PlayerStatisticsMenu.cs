using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using TheRockPaperScissorsGame.Client.Contracts;
using TheRockPaperScissorsGame.Client.Clients;
using System.Threading;

namespace TheRockPaperScissorsGame.Client.Menu
{
    public class PlayerStatisticsMenu : IMenu
    {
        private UserClient _userClient;
        private GameClient _gameClient;
        private StatisticClient _statisticClient;

        public PlayerStatisticsMenu(UserClient userClient, GameClient gameClient, StatisticClient statisticClient)
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
                HttpResponseMessage result;
                string content;

                var options = new string[] { "Total results", "Total playing time", "Moves statistics", "Back" };
                var command = MenuLibrary.InputMenuItemNumber("PlayerStatistics", options);
                switch (command)
                {
                    case 1:
                        result = await _statisticClient.GetUserResultsAsync();

                        content = await result.Content.ReadAsStringAsync();

                        var results = JsonSerializer.Deserialize<ResultsDto>(content);

                        Console.Clear();

                        Console.WriteLine("Your statistics: ");

                        Console.WriteLine($"Wins: {results.WinCount}");
                        Console.WriteLine($"Draws: {results.DrawCount}");
                        Console.WriteLine($"Losses: {results.LossCount}");

                        Console.WriteLine("Press any key to return to the player statistics menu");
                        Console.ReadKey();
                        break;
                    case 2:
                        result = await _statisticClient.GetUserGameTime();
                        // Another method to show the information?\
                        Console.Clear();

                        Console.WriteLine($"Time, spended in the game:  {await result.Content.ReadAsStringAsync()}");

                        Console.WriteLine("Press any key to return to the player statistics menu");
                        Console.ReadKey();
                        break;
                    case 3:
                        result = await _statisticClient.GetUserMovesAsync();

                        content = await result.Content.ReadAsStringAsync();

                        var movesResults = JsonSerializer.Deserialize<MovesDto>(content);

                        Console.Clear();

                        Console.WriteLine($"Rock: {movesResults.RockCount}");
                        Console.WriteLine($"Scissors: {movesResults.PaperCount}");
                        Console.WriteLine($"Paper: {movesResults.ScissorsCount}");

                        Console.WriteLine("Press any key to return to the player statistics menu");
                        Console.ReadKey();

                        break;
                    case 4:
                    default:
                        return;
                }
            }
        }
    }
}
