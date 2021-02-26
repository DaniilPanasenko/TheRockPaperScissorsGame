using System;
using System.Text.Json;
using System.Threading.Tasks;
using TheRockPaperScissorsGame.Client.Contracts;
using TheRockPaperScissorsGame.Client.Clients;
using TheRockPaperScissorsGame.Client.Menu.Library;
using System.Net;
using Microsoft.Extensions.Logging;

namespace TheRockPaperScissorsGame.Client.Menu
{
    public class PlayerStatisticsMenu : IMenu
    {
        private readonly StatisticClient _statisticClient;

        private readonly IntervalResultsMenu _intervalResultsMenu;

        private readonly ILogger<PlayerStatisticsMenu> _logger;

        public PlayerStatisticsMenu(StatisticClient statisticClient, IntervalResultsMenu intervalResultsMenu, ILogger<PlayerStatisticsMenu> logger)
        {
            _statisticClient = statisticClient;
            _intervalResultsMenu = intervalResultsMenu;
            _logger = logger;
        }

        public async Task StartAsync()
        {
            _logger.LogInformation("class PlayerStatisticsMenu. StartAsync()");

            while (true)
            {
                _logger.LogInformation("Choosing the Statistics Type");

                MenuLibrary.Clear();

                var options = new string[] { "Total results", "Total playing time", "Moves statistics", "Results by the time", "Back" };
                var command = MenuLibrary.InputMenuItemNumber("Player statistics Menu", options);

                _logger.LogInformation("Chose the command");

                switch (command)
                {
                    case 1:
                        await GetResults();
                        break;
                    case 2:
                        await GetTotalTime();
                        break;
                    case 3:
                        await GetMovesStatistics();
                        break;
                    case 4:
                        await _intervalResultsMenu.StartAsync();
                        break;
                    case 5:
                        return;
                }
            }
        }

        private async Task GetResults()
        {
            _logger.LogInformation("class PlayerStatisticsMenu. GetResults()");

            var response = await _statisticClient.GetUserResultsAsync();

            _logger.LogInformation("REQUEST: Sent the request to get the statistics");

            if (response.StatusCode == HttpStatusCode.OK)
            {
                _logger.LogInformation("RESPONSE: Get the Statistics (200)");

                var content = await response.Content.ReadAsStringAsync();
                var results = JsonSerializer.Deserialize<ResultsDto>(content);

                MenuLibrary.WriteLineColor("\nYour results:\n", ConsoleColor.Yellow);

                MenuLibrary.WriteColor("Wins: ", ConsoleColor.Yellow);
                MenuLibrary.WriteLineColor(results.WinCount.ToString(), ConsoleColor.White);

                MenuLibrary.WriteColor("Draws: ", ConsoleColor.Yellow);
                MenuLibrary.WriteLineColor(results.DrawCount.ToString(), ConsoleColor.White);

                MenuLibrary.WriteColor("Losses: ", ConsoleColor.Yellow);
                MenuLibrary.WriteLineColor(results.LossCount.ToString(), ConsoleColor.White);

                MenuLibrary.PressAnyKey();
            }
            else
            {
                _logger.LogInformation("RESPONSE: Unknown response");
                throw new HttpListenerException();
            }
        }

        private async Task GetTotalTime()
        {
            _logger.LogInformation("class PlayerStatisticsMenu. GetTotalTime()");

            var response = await _statisticClient.GetUserGameTimeAsync();

            _logger.LogInformation("REQUEST: Sent the request to get the statistics");

            if (response.StatusCode == HttpStatusCode.OK)
            {
                _logger.LogInformation("RESPONSE: Get the Statistics (200)");

                var time = await response.Content.ReadAsStringAsync();
                time = JsonSerializer.Deserialize<string>(time);

                MenuLibrary.WriteColor("\nTotal time in the game: ", ConsoleColor.Yellow);
                MenuLibrary.WriteLineColor(time, ConsoleColor.White);

                MenuLibrary.PressAnyKey();
            }
            else
            {
                _logger.LogInformation("RESPONSE: Unknown response");
                throw new HttpListenerException();
            }
        }

        private async Task GetMovesStatistics()
        {
            _logger.LogInformation("class PlayerStatisticsMenu. GetMovesStatistics()");

            var response = await _statisticClient.GetUserMovesAsync();

            _logger.LogInformation("REQUEST: Sent the request to get the statistics");

            if (response.StatusCode == HttpStatusCode.OK)
            {
                _logger.LogInformation("RESPONSE: Get the Statistics (200)");

                var content = await response.Content.ReadAsStringAsync();
                var results = JsonSerializer.Deserialize<MovesDto>(content);

                MenuLibrary.WriteLineColor("\nYour moves:\n", ConsoleColor.Yellow);

                MenuLibrary.WriteColor("Rock: ", ConsoleColor.Yellow);
                MenuLibrary.WriteLineColor(results.RockCount.ToString(), ConsoleColor.White);

                MenuLibrary.WriteColor("Paper: ", ConsoleColor.Yellow);
                MenuLibrary.WriteLineColor(results.PaperCount.ToString(), ConsoleColor.White);

                MenuLibrary.WriteColor("Scissors: ", ConsoleColor.Yellow);
                MenuLibrary.WriteLineColor(results.ScissorsCount.ToString(), ConsoleColor.White);

                MenuLibrary.PressAnyKey();
            }
            else
            {
                _logger.LogInformation("RESPONSE: Unknown response");
                throw new HttpListenerException();
            }
        }
    }
}