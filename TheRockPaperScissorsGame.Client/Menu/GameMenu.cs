using Microsoft.Extensions.Logging;
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
        private readonly GameClient _gameClient;

        private readonly SessionResults _sessionResults;

        private Move _currentMove;

        private readonly ILogger<GameMenu> _logger;

        public GameMenu(GameClient gameClient, ILogger<GameMenu> logger)
        {
            _gameClient = gameClient;
            _logger = logger;
            _sessionResults = new SessionResults();
        }

        public async Task StartAsync()
        {
            _logger.LogInformation("In the GameMenu");

            while (true)
            {
                _logger.LogInformation("Choosing the move");

                PrintHeader();

                var options = new string[] { "Rock", "Paper", "Scissors", "Quit" };
                var command = MenuLibrary.InputMenuItemNumber("Choose command", options);

                _logger.LogInformation("Chose the command");

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
                        _logger.LogInformation("Sending the request to finish the session");
                        await _gameClient.FinishSessionAsync();
                        return;
                }

                var success = await DoMoveAsync(move);
                if (!success)
                {
                    return;
                }

                success = await CheckMoveAsync();
                if (!success)
                {
                    return;
                }
            }
        }

        public async Task<bool> DoMoveAsync(Move move)
        {
            _logger.LogInformation("Doing the move");

            var response = await _gameClient.DoMoveAsync(move);

            _logger.LogInformation("Sent the request to the server to do the move");

            if(response.StatusCode== HttpStatusCode.OK)
            {
                _logger.LogInformation("Successful response (200)");

                _currentMove = move;
                PrintHeader();
                MenuLibrary.WriteColor($"\nYour move: ", ConsoleColor.White);
                MenuLibrary.WriteLineColor($"{_currentMove}", ConsoleColor.DarkCyan);
                return true;
            }
            else if (response.StatusCode == HttpStatusCode.Conflict)
            {
                _logger.LogInformation("Game is over (409)");

                await ResponseLibrary.GameFinishedResponseAsync(response);
            }
            else
            {
                _logger.LogInformation("Unknown response");

                throw new HttpListenerException();
            }

            return false;
        }

        public async Task<bool> CheckMoveAsync()
        {
            _logger.LogInformation("Checking the move");

            MenuLibrary.WriteLineColor("\nWait opponent...", ConsoleColor.DarkCyan);
            while (true)
            {
                var response = await _gameClient.CheckMoveAsync();

                _logger.LogInformation("Sent the request to check the move");

                if (response.StatusCode == HttpStatusCode.OK)
                {

                    _logger.LogInformation("Player did a move (200)");

                    var resultJson = await response.Content.ReadAsStringAsync();
                    var result = JsonSerializer.Deserialize<RoundResultDto>(resultJson);
                    PrintResult(result);
                    return true;
                }
                else if (response.StatusCode == HttpStatusCode.Conflict)
                {
                    _logger.LogInformation("Game is over (409)");

                    await ResponseLibrary.GameFinishedResponseAsync(response);
                    return false;
                }
                else if(response.StatusCode != HttpStatusCode.NotFound)
                {
                    _logger.LogInformation("Unknown response");

                    throw new HttpListenerException();
                }

                _logger.LogInformation("The opponent did not do the move");
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