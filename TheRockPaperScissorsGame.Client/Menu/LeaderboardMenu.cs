using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using TheRockPaperScissorsGame.Client.Clients;
using TheRockPaperScissorsGame.Client.Contracts;
using TheRockPaperScissorsGame.Client.Contracts.Enums;

namespace TheRockPaperScissorsGame.Client.Menu
{
    public class LeaderboardMenu : IMenu
    {
        private UserClient _userClient;
        private GameClient _gameClient;
        private StatisticClient _statisticClient;

        public LeaderboardMenu(UserClient userClient, GameClient gameClient, StatisticClient statisticClient)
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

                HttpResponseMessage response;
                string content;

                StatisticsType statisticsType;
                int amount;

                var options = new string[] { "Win statistics leaderboard", "Time statistics leaderboard", "Wins statistics(in persents) leaderboard", "Back" };
                var command = MenuLibrary.InputMenuItemNumber("Leaderboard", options);
                switch (command)
                {
                    case 1:
                        statisticsType = StatisticsType.WinStatistics;
                        amount = GetAmount();

                        response = await _statisticClient.GetLeaderboardAsync(amount, statisticsType);
                        content = await response.Content.ReadAsStringAsync();

                        var winResults = JsonSerializer.Deserialize<List<UserResultDto<int>>>(content);
                        break;
                    case 2:
                        statisticsType = StatisticsType.TimeStatistics;
                        amount = GetAmount();

                        response = await _statisticClient.GetLeaderboardAsync(amount, statisticsType);
                        content = await response.Content.ReadAsStringAsync();

                        var timeResults = JsonSerializer.Deserialize<List<UserResultDto<string>>>(content);

                        break;
                    case 3:
                        statisticsType = StatisticsType.WinPercentStatistics;
                        amount = GetAmount();

                        response = await _statisticClient.GetLeaderboardAsync(amount, statisticsType);
                        content = await response.Content.ReadAsStringAsync();

                        var winPersResults = JsonSerializer.Deserialize<List<UserResultDto<decimal>>>(content);
                        break;
                    case 4:
                    default:
                        return;
                }
            }
            throw new NotImplementedException();
        }

        private int GetAmount()
        {
            while (true)
            {
                Console.WriteLine("Please, choose: 1.Set the amount of best players in the rate\n2.Show all players in the rate");
                string choice = Console.ReadLine();

                if (choice == "1")
                {
                    while (true)
                    {
                        try
                        {
                            Console.Write("Amount: ");
                            return Convert.ToInt32(Console.ReadLine());
                        }
                        catch (FormatException)
                        {
                            Console.WriteLine("Wrong format. Please, try again");
                        }
                    }
                }
                else
                if(choice=="2")
                {
                    return int.MaxValue;
                }
                else 
                {
                    Console.WriteLine("Wrong option, please, try again");
                }
            }
        }

        

        private void ShowRate()
        { 
        
        }
    }
}
