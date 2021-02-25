using System;
using System.Net;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using TheRockPaperScissorsGame.Client.Clients;
using TheRockPaperScissorsGame.Client.Contracts.Enums;
using TheRockPaperScissorsGame.Client.Menu.Library;

namespace TheRockPaperScissorsGame.Client.Menu
{
    public class GameMenu : IMenu
    {
        private UserClient _userClient;
        private GameClient _gameClient;
        private StatisticClient _statisticClient;

        public GameMenu(UserClient userClient, GameClient gameClient, StatisticClient statisticClient)
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
                MenuLibrary.WriteLineColor("\nGame\n", ConsoleColor.Yellow);

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

                Thread.Sleep(2000);
            }
        }

        public async Task<bool> DoMove(Move move)
        {
            var response = await _gameClient.DoMove(move);
            if(response.StatusCode== HttpStatusCode.OK)
            {
                MenuLibrary.WriteColor($"\nYour move: {move}\n", ConsoleColor.White);
                MenuLibrary.WriteLineColor($"{move}\n", ConsoleColor.Green);
                return true;
            }
            else if (response.StatusCode == HttpStatusCode.Conflict)
            {
                await ResponseLibrary.GameFinishedResponseAsync(response);
            }
            else
            {
                ResponseLibrary.UnknownResponse();
            }
            return false;
        }

        public async Task<bool> CheckMove()
        {
            MenuLibrary.WriteLineColor("Wait opponent...", ConsoleColor.DarkCyan);
            while (true)
            {
                var response = await _gameClient.CheckMove();
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var move = await response.Content.ReadAsStringAsync();
                    move = JsonSerializer.Deserialize<string>(move);

                    MenuLibrary.WriteColor($"\nOpponent move: {move}\n", ConsoleColor.White);
                    MenuLibrary.WriteLineColor($"{move}\n", ConsoleColor.Green);
                    return true;
                }
                else if (response.StatusCode == HttpStatusCode.Conflict)
                {
                    await ResponseLibrary.GameFinishedResponseAsync(response);
                    return false;
                }
                else if(response.StatusCode != HttpStatusCode.NotFound)
                {
                    ResponseLibrary.UnknownResponse();
                    return false;
                }
                Thread.Sleep(200);
            }
        }
    }
}
