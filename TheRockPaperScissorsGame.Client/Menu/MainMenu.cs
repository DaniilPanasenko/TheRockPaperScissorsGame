using System;
using System.Threading;
using System.Threading.Tasks;
using TheRockPaperScissorsGame.Client.Clients;

namespace TheRockPaperScissorsGame.Client.Menu
{
    public class MainMenu : IMenu
    {
        private UserClient _userClient;
        private GameClient _gameClient;
        private StatisticClient _statisticClient;

        public MainMenu(UserClient userClient, GameClient gameClient, StatisticClient statisticClient)
        {
            _userClient = userClient;
            _gameClient = gameClient;
            _statisticClient = statisticClient;
        }

        public async Task StartAsync()
        {
            while (true)
            {

                IMenu menu;

                var options = new string[] { "Authorization", "Leaderboard", "Exit" };

                var command = MenuLibrary.InputMenuItemNumber("Main", options);
                switch (command)
                {
                    case 1:
                        menu = new AuthorizationMenu(_userClient, _gameClient, _statisticClient);
                        await menu.StartAsync();
                        break;
                    case 2:
                        menu = new LeaderboardMenu(_userClient, _gameClient, _statisticClient);
                        await menu.StartAsync();
                        break;
                    case 3:
                    default:
                        return;
                }
            }
        }
    }
}