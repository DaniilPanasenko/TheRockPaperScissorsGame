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
        private UserClient _userClient;
        private GameClient _gameClient;
        private StatisticClient _statisticClient;

        public IntervalResultsMenu(UserClient userClient, GameClient gameClient, StatisticClient statisticClient)
        {
            _userClient = userClient;
            _gameClient = gameClient;
            _statisticClient = statisticClient;
        }

        public async Task StartAsync()
        {
            while (true)
            {
                TimeInterval timeInterval = TimeInterval.Day;

                MenuLibrary.Clear();

                var options = new string[] { "By days", "By hours", "By minutes", "Back" };
                var command = MenuLibrary.InputMenuItemNumber("Leaderboard Menu", options);

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

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var content = await response.Content.ReadAsStringAsync();
                return content;
            }
            else
            {
                ResponseLibrary.UnknownResponse();
                return null;
            }
        }

        private void ShowRate(List<ResultByTimeDto> results)
        {
            MenuLibrary.WriteLineColor("", ConsoleColor.White);
            int place = 1;
            foreach (var result in results)
            {
                MenuLibrary.WriteColor($"{place}. {result.Time}->", ConsoleColor.White);
                MenuLibrary.WriteLineColor(result.WinCount.ToString() + " : ", ConsoleColor.Green);
                MenuLibrary.WriteLineColor(result.LossCount.ToString(), ConsoleColor.Red);
                place++;
            }
            MenuLibrary.PressAnyKey();
        }
    }
}
