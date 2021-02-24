using System;
using System.Threading.Tasks;
using TheRockPaperScissorsGame.Client.Clients;

namespace TheRockPaperScissorsGame.Client.Menu
{
    public class UserMenu : IMenu
    {
        private UserClient _userClient;
        private GameClient _gameClient;
        private StatisticClient _statisticClient;

        public UserMenu(UserClient userClient, GameClient gameClient, StatisticClient statisticClient)
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
                var options = new string[] { "Game", "Statistics", "Logout" };
                var command = MenuLibrary.InputMenuItemNumber("User", options);
                switch (command)
                {
                    case 1:
                        IMenu menu = new GameStartMenu(_userClient, _gameClient, _statisticClient);
                        await menu.StartAsync();
                        break;
                    case 2:
                        //
                        break;
                    case 3:
                    default:
                        return;
                }
            }
        }
    }
}
