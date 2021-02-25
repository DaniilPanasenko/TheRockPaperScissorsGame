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
        private UserClient _userClient;
        private GameClient _gameClient;
        private StatisticClient _statisticClient;

        public GameStartMenu(UserClient userClient, GameClient gameClient, StatisticClient statisticClient)
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

                var options = new string[] { "Public game", "Private game", "Train game", "Back" };
                var command = MenuLibrary.InputMenuItemNumber("Game Menu", options);

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
            var response = await _gameClient.StartSessionAsync(roomType, roomId);

            if (response.StatusCode == HttpStatusCode.OK)
            {
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
                await ResponseLibrary.RepeatOperationWithMessageAsync<string>(response);
            }
            else
            {
                throw new HttpListenerException();
            }
        }

        private async Task WaitPlayerAsync()
        {
            while (true)
            {
                var response = await _gameClient.CheckSessionAsync();

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var name = await response.Content.ReadAsStringAsync();
                    name = JsonSerializer.Deserialize<string>(name);

                    MenuLibrary.WriteLineColor($"\nYour opponent is {name}\n", ConsoleColor.Green);
                    MenuLibrary.PressAnyKey();

                    IMenu menu = new GameMenu(_userClient, _gameClient, _statisticClient);
                    await menu.StartAsync();
                    return;
                }
                else if(response.StatusCode == HttpStatusCode.Conflict)
                {
                    await ResponseLibrary.GameFinishedResponseAsync(response);
                    return;
                }
                else if(response.StatusCode != HttpStatusCode.NotFound)
                {
                    throw new HttpListenerException();
                }

                Thread.Sleep(200);
            }
        }
    }
}
