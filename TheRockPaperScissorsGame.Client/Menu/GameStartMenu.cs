using Microsoft.Extensions.Logging;
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
    public class GameStartMenu : IMenu
    {
        private readonly GameClient _gameClient;
        private readonly GameMenu _gameMenu;
        private readonly ILogger<GameStartMenu> _logger;

        public GameStartMenu(GameClient gameClient, GameMenu gameMenu, ILogger<GameStartMenu> logger)
        {
            _gameClient = gameClient;
            _gameMenu = gameMenu;
            _logger = logger;
        }

        public async Task StartAsync()
        {
            _logger.LogInformation("In the GameStartMenu");

            while (true)
            {
                _logger.LogInformation("Choosing the game type");

                MenuLibrary.Clear();

                var options = new string[] { "Public game", "Private game", "Train game", "Back" };
                var command = MenuLibrary.InputMenuItemNumber("Game Menu", options);

                _logger.LogInformation("Chose the command");

                RoomType roomType;
                string roomId = null;

                switch (command)
                {
                    case 1:
                        roomType = RoomType.Public;
                        break;
                    case 2:
                        roomType = RoomType.Private;
                        roomId = ChoosePrivateRoom();
                        break;
                    case 3:
                        roomType = RoomType.Train;
                        break;
                    case 4:
                    default:
                        return;
                }

                await GameStartAsync(roomType, roomId);
            }
        }

        private string ChoosePrivateRoom()
        {
            var options = new string[] { "Start private room", "Connect to private room" };
            var command = MenuLibrary.InputMenuItemNumber("Private room Menu", options);

            if (command == 1)
            {
                return null;
            }
            else
            {
                return MenuLibrary.InputStringValue("room number");
            }
        }

        private async Task GameStartAsync(RoomType roomType, string roomId)
        {
            _logger.LogInformation("Starting the game");

            var response = await _gameClient.StartSessionAsync(roomType, roomId);

            _logger.LogInformation("Sent the request to the server to start the game");

            if (response.StatusCode == HttpStatusCode.OK)
            {
                _logger.LogInformation("Game successfully started");

                MenuLibrary.WriteLineColor("\nSuccessfully started game\n", ConsoleColor.Green);

                if (roomType == RoomType.Private && roomId == null)
                {
                    var roomNumber = await response.Content.ReadAsStringAsync();
                    roomNumber = JsonSerializer.Deserialize<string>(roomNumber);

                    MenuLibrary.WriteColor("Your room number: ", ConsoleColor.White);
                    MenuLibrary.WriteLineColor(roomNumber, ConsoleColor.Green);
                    MenuLibrary.WriteLineColor("Say it your friend to connection.", ConsoleColor.White);
                }
                MenuLibrary.WriteLineColor("\nWait for the opponent...\n", ConsoleColor.DarkCyan);

                await WaitPlayerAsync();
            }
            else if (response.StatusCode == HttpStatusCode.BadRequest ||
                     response.StatusCode == HttpStatusCode.Conflict)
            {

                _logger.LogInformation("Did not manage to start the game (400, 409)");
                await ResponseLibrary.RepeatOperationWithMessageAsync<string>(response);
            }
            else
            {
                _logger.LogInformation("Unknown response");
                throw new HttpListenerException();
            }
        }

        private async Task WaitPlayerAsync()
        {
            _logger.LogInformation("Waiting for the opponent");
            while (true)
            {
                var response = await _gameClient.CheckSessionAsync();

                _logger.LogInformation("Sent the request to check the opponent");

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    _logger.LogInformation("Opponent was found (200)");

                    var name = await response.Content.ReadAsStringAsync();
                    name = JsonSerializer.Deserialize<string>(name);

                    MenuLibrary.WriteLineColor($"\nYour opponent is {name}\n", ConsoleColor.Green);
                    Thread.Sleep(2000);

                    await _gameMenu.StartAsync();
                    return;
                }
                else if(response.StatusCode == HttpStatusCode.Conflict)
                {
                    _logger.LogInformation("Game is over (409)");
                    await ResponseLibrary.GameFinishedResponseAsync(response);
                    return;
                }
                else if(response.StatusCode != HttpStatusCode.NotFound)
                {
                    _logger.LogInformation("Unknown response");
                    throw new HttpListenerException();
                }
                _logger.LogInformation("Opponnent was not found (404)");
                Thread.Sleep(200);
            }
        }
    }
}