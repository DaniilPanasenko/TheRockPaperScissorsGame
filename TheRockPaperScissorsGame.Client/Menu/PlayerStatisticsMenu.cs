using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using TheRockPaperScissorsGame.Client.Contracts;
using TheRockPaperScissorsGame.Client.Clients;
using System.Threading;
using TheRockPaperScissorsGame.Client.Menu.Library;
using System.Net;

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
                MenuLibrary.Clear();

                var options = new string[] { "Total results", "Total playing time", "Moves statistics", "Results by the time", "Back" };
                var command = MenuLibrary.InputMenuItemNumber("Player statistics Menu", options);

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
                        //
                        break;
                    case 5:
                        return;
                }
            }
        }

        private async Task GetResults()
        {
            var response = await _statisticClient.GetUserResultsAsync();
            if (response.StatusCode == HttpStatusCode.OK)
            {
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
                throw new HttpListenerException();
            }
        }

        private async Task GetTotalTime()
        {
            var response = await _statisticClient.GetUserGameTime();
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var time = await response.Content.ReadAsStringAsync();
                time = JsonSerializer.Deserialize<string>(time);

                MenuLibrary.WriteColor("\nTotal time in the game: ", ConsoleColor.Yellow);
                MenuLibrary.WriteLineColor(time, ConsoleColor.White);

                MenuLibrary.PressAnyKey();
            }
            else
            {
                throw new HttpListenerException();
            }
        }

        private async Task GetMovesStatistics()
        {
            var response = await _statisticClient.GetUserMovesAsync();
            if (response.StatusCode == HttpStatusCode.OK)
            {
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
                throw new HttpListenerException();
            }
        }
    }
}
