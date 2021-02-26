using System.Threading.Tasks;
using TheRockPaperScissorsGame.Client.Clients;
using TheRockPaperScissorsGame.Client.Menu.Library;

namespace TheRockPaperScissorsGame.Client.Menu
{
    public class UserMenu : IMenu
    {
        private readonly GameClient _gameClient;
        private readonly StatisticClient _statisticClient;

        public UserMenu(GameClient gameClient, StatisticClient statisticClient)
        {
            _gameClient = gameClient;
            _statisticClient = statisticClient;
        }

        public async Task StartAsync()
        {
            while (true)
            {
                IMenu menu;

                MenuLibrary.Clear();

                var options = new string[] { "Game", "Statistics", "Logout" };
                var command = MenuLibrary.InputMenuItemNumber("User Menu", options);

                switch (command)
                {
                    case 1:
                        menu = new GameStartMenu(_gameClient);
                        await menu.StartAsync();
                        break;
                    case 2:
                        menu = new StatisticsMenu(_statisticClient);
                        await menu.StartAsync();
                        break;
                    case 3:
                        return;
                }
            }
        }
    }
}
