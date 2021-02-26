using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using TheRockPaperScissorsGame.Client.Clients;
using TheRockPaperScissorsGame.Client.Contracts;
using TheRockPaperScissorsGame.Client.Contracts.Enums;
using TheRockPaperScissorsGame.Client.Menu.Library;

namespace TheRockPaperScissorsGame.Client.Menu
{
    public class IntervalResultsMenu : IMenu
    {
        private readonly StatisticClient _statisticClient;
        private readonly ILogger<IntervalResultsMenu> _logger;

        public IntervalResultsMenu(StatisticClient statisticClient, ILogger<IntervalResultsMenu> logger)
        {
            _statisticClient = statisticClient;
            _logger = logger;
        }

        public async Task StartAsync()
        {
            _logger.LogInformation("In the IntervalResultsMenu");

            while (true)
            {
                _logger.LogInformation("Choosing Time Interval");

                TimeInterval timeInterval = TimeInterval.Day;

                MenuLibrary.Clear();

                var options = new string[] { "By days", "By hours", "By minutes", "Back" };
                var command = MenuLibrary.InputMenuItemNumber("Leaderboard Menu", options);

                _logger.LogInformation("Chose the command");

                switch (command)
                {
                    case 1:
                        timeInterval = TimeInterval.Day;
                        break;
                    case 2:
                        timeInterval = TimeInterval.Hour;
                        break;
                    case 3:
                        timeInterval = TimeInterval.Minute;
                        break;
                    case 4:
                        return;
                }

                var content = await GetResultByIntervalAsync(timeInterval);

                if (content == null)
                {
                    continue;
                }

                var results = JsonSerializer.Deserialize<List<ResultByTimeDto>>(content);
                ShowRate(results);
            }
        }

        private async Task<string> GetResultByIntervalAsync(TimeInterval timeInterval)
        {
            var options = new string[] { "Set the amount of last days/hours/minutes for statistics", "Show all days/hours/minutes for statistics" };
            var command = MenuLibrary.InputMenuItemNumber("Please, choose", options);

            var amount = command == 1
                ? MenuLibrary.InputIntegerValue("amount", 1, int.MaxValue)
                : int.MaxValue;

            var response = await _statisticClient.GetUserResultByIntervalAsync(amount, timeInterval);

            _logger.LogInformation("Sent the response to get the statistics");

            if (response.StatusCode == HttpStatusCode.OK)
            {
                _logger.LogInformation("Get the statistics (200)");

                var content = await response.Content.ReadAsStringAsync();
                return content;
            }
            else
            {
                _logger.LogInformation("Unknown response");
                throw new HttpListenerException();
            }
        }

        private void ShowRate(List<ResultByTimeDto> results)
        {
            MenuLibrary.WriteLineColor("", ConsoleColor.White);
            int place = 1;
            foreach (var result in results)
            {
                MenuLibrary.WriteColor($"{place}. {result.Time} -> ", ConsoleColor.White);
                MenuLibrary.WriteColor(result.WinCount.ToString(), ConsoleColor.Green);
                MenuLibrary.WriteColor(" : ", ConsoleColor.White);
                MenuLibrary.WriteLineColor(result.LossCount.ToString(), ConsoleColor.Red);
                place++;
            }
            MenuLibrary.PressAnyKey();
        }
    }
}