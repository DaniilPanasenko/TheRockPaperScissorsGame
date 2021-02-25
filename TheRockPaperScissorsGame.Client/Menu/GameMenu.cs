using System;
using System.Net;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using TheRockPaperScissorsGame.Client.Clients;
using TheRockPaperScissorsGame.Client.Contracts;
using TheRockPaperScissorsGame.Client.Contracts.Enums;
using TheRockPaperScissorsGame.Client.Menu.Library;
using TheRockPaperScissorsGame.Client.Models;

namespace TheRockPaperScissorsGame.Client.Menu
{
    public class GameMenu : IMenu
    {
        private UserClient _userClient;
        private GameClient _gameClient;
        private StatisticClient _statisticClient;

        private SessionResults _sessionResults;
        private Move _currentMove; 

        public GameMenu(UserClient userClient, GameClient gameClient, StatisticClient statisticClient)
        {
            _userClient = userClient;
            _gameClient = gameClient;
            _statisticClient = statisticClient;
            _sessionResults = new SessionResults();
        }

        public async Task StartAsync()
        {
            while (true)
            {
                PrintHeader();

                var options = new string[] { "Rock", "Paper", "Scissors", "Quit" };
                var command = MenuLibrary.InputMenuItemNumber("Choose command", options);

                Move move = Move.Rock;
                switch (command)
                {
                    case 1:
                        move = Move.Rock;
                        break;
                    case 2:
                        move = Move.Paper;
                        break;
                    case 3:
                        move = Move.Scissors;
                        break;
                    case 4:
                        await _gameClient.FinishSession();
                        return;
                }

                var success = await DoMove(move);
                if (!success) return;

                success = await CheckMove();
                if (!success) return;
            }
        }

        public async Task<bool> DoMove(Move move)
        {
            var response = await _gameClient.DoMove(move);
            if(response.StatusCode== HttpStatusCode.OK)
            {
                _currentMove = move;
                PrintHeader();
                MenuLibrary.WriteColor($"\nYour move: ", ConsoleColor.White);
                MenuLibrary.WriteLineColor($"{_currentMove}", ConsoleColor.DarkCyan);
                return true;
            }
            else if (response.StatusCode == HttpStatusCode.Conflict)
            {
                await ResponseLibrary.GameFinishedResponseAsync(response);
            }
            else
            {
                throw new HttpListenerException();
            }
            return false;
        }

        public async Task<bool> CheckMove()
        {
            MenuLibrary.WriteLineColor("\nWait opponent...", ConsoleColor.DarkCyan);
            while (true)
            {
                var response = await _gameClient.CheckMove();
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var resultJson = await response.Content.ReadAsStringAsync();
                    var result = JsonSerializer.Deserialize<RoundResultDto>(resultJson);
                    PrintResult(result);
                    return true;
                }
                else if (response.StatusCode == HttpStatusCode.Conflict)
                {
                    await ResponseLibrary.GameFinishedResponseAsync(response);
                    return false;
                }
                else if(response.StatusCode != HttpStatusCode.NotFound)
                {
                    throw new HttpListenerException();
                }
                Thread.Sleep(200);
            }
        }

        public void PrintHeader()
        {
            MenuLibrary.Clear();
            MenuLibrary.WriteLineColor("\nGame\n", ConsoleColor.Yellow);
            MenuLibrary.WriteColor("Results: ", ConsoleColor.White);
            MenuLibrary.WriteColor($"{_sessionResults.WinCount} : ", ConsoleColor.Green);
            MenuLibrary.WriteColor($"{_sessionResults.DrawCount} : ", ConsoleColor.Yellow);
            MenuLibrary.WriteLineColor($"{_sessionResults.LossesCount}", ConsoleColor.Red);
        }

        public void PrintResult(RoundResultDto result)
        {
            string resultString = "";
            ConsoleColor color = ConsoleColor.White;

            switch (result.Result)
            {
                case 1:
                    _sessionResults.WinCount++;
                    resultString = "Win";
                    color = ConsoleColor.Green;
                    break;
                case 0:
                    _sessionResults.DrawCount++;
                    resultString = "Draw";
                    color = ConsoleColor.Yellow;
                    break;
                case -1:
                    _sessionResults.LossesCount++;
                    resultString = "Lose";
                    color = ConsoleColor.Red;
                    break;
            }

            PrintHeader();

            MenuLibrary.WriteColor($"\nYour move: ", ConsoleColor.White);
            MenuLibrary.WriteLineColor($"{_currentMove}", ConsoleColor.DarkCyan);

            MenuLibrary.WriteColor($"Opponent move: ", ConsoleColor.White);
            MenuLibrary.WriteLineColor($"{result.OpponentMove}", ConsoleColor.DarkCyan);

            MenuLibrary.WriteColor($"\nResult: ", ConsoleColor.White);
            MenuLibrary.WriteLineColor(resultString, color);

            MenuLibrary.PressAnyKey();
        }
    }
}
