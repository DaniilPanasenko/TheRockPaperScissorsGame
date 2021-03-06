﻿using Microsoft.Extensions.Logging;
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
    public class LeaderboardMenu : IMenu
    {
        private readonly StatisticClient _statisticClient;

        private readonly ILogger<LeaderboardMenu> _logger;

        public LeaderboardMenu(StatisticClient statisticClient, ILogger<LeaderboardMenu> logger)
        {
            _statisticClient = statisticClient;
            _logger = logger;
        }

        public async Task StartAsync()
        {
            _logger.LogInformation("class LeaderboardMenu. StartAsync()");
            
            while (true)
            {
                _logger.LogInformation("Choosing Statistics Type");

                StatisticsType statisticsType = StatisticsType.WinStatistics;

                MenuLibrary.Clear();

                var options = new string[] { "Win statistics", "Time statistics", "Wins statistics(in persents)", "Back" };
                var command = MenuLibrary.InputMenuItemNumber("Leaderboard Menu", options);

                _logger.LogInformation("Chose the command");

                switch (command)
                {
                    case 1:
                        statisticsType = StatisticsType.WinStatistics;
                        break;
                    case 2:
                        statisticsType = StatisticsType.TimeStatistics;
                        break;
                    case 3:
                        statisticsType = StatisticsType.WinPercentStatistics;
                        break;
                    case 4:
                        return;
                }

                var content = await GetStatistcsAsync(statisticsType);
                if (content == null)
                {
                    continue;
                }

                var results = JsonSerializer.Deserialize<List<UserResultDto>>(content);
                ShowRate(results);
            }
        }

        private async Task<string> GetStatistcsAsync(StatisticsType type)
        {
            _logger.LogInformation("class LeaderboardMenu. GetStatistcsAsync()");

            var options = new string[] { "Set the amount of best players in the rate", "Show all players in the rate" };
            var command = MenuLibrary.InputMenuItemNumber("Please, choose", options);

            var amount = command == 1
                ? MenuLibrary.InputIntegerValue("amount", 1, int.MaxValue)
                : int.MaxValue;

            var response = await _statisticClient.GetLeaderboardAsync(amount, type);

            _logger.LogInformation("REQUEST: Sent the request for statistics");

            if (response.StatusCode == HttpStatusCode.OK)
            {
                _logger.LogInformation("RESPONSE: Got the statistics (200)");

                var content = await response.Content.ReadAsStringAsync();
                return content;
            }
            else
            {
                _logger.LogInformation("RESPONSE: Unknown response");
                throw new HttpListenerException();
            }
        }

        private void ShowRate(List<UserResultDto> results)
        {
            MenuLibrary.WriteLineColor("", ConsoleColor.White);
            int place = 1;
            foreach(var result in results)
            {
                MenuLibrary.WriteColor($"{place}. {result.Login}: ", ConsoleColor.White);
                MenuLibrary.WriteLineColor(result.Result, ConsoleColor.Yellow);
                place++;
            }
            MenuLibrary.PressAnyKey();
        }
    }
}