using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using TheRockPaperScissorsGame.Client.Clients;
using TheRockPaperScissorsGame.Client.Contracts.Enums;
using TheRockPaperScissorsGame.Client.Services;

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
                Console.Clear();
                var options = new string[] { "Public game", "Private game", "Train game", "Back" };
                var command = MenuLibrary.InputMenuItemNumber("Game", options);
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
                var response = await _gameClient.StartSession(roomType, roomId);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    MenuLibrary.WriteLineColor("\nSuccessfully started game\n", ConsoleColor.Green);
                    Thread.Sleep(500);
                    //IMenu menu = new UserMenu();
                    //await menu.StartAsync();
                    return;
                }
                else if (response.StatusCode == HttpStatusCode.BadRequest ||
                         response.StatusCode == HttpStatusCode.Conflict)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    var textResponse = ResponseDeserializer.Deserialize<string>(jsonResponse);
                    MenuLibrary.WriteLineColor($"\n{textResponse}", ConsoleColor.Red);
                    Console.WriteLine("Please repeat starting game.");
                    Thread.Sleep(3000);
                }
                else
                {
                    MenuLibrary.WriteLineColor("\nSorry, something went wrong, try it later\n", ConsoleColor.Red);
                    Thread.Sleep(3000);
                    return;
                }
            }
        }

        private string ChoosePrivateRoom()
        {
            var options = new string[] { "Start private room", "Connect to private room" };
            var command = MenuLibrary.InputMenuItemNumber("Private room", options);
            if (command == 1)
            {
                return null;
            }
            else
            {
                return MenuLibrary.InputStringValue("room number");
            }
        }
    }
}
