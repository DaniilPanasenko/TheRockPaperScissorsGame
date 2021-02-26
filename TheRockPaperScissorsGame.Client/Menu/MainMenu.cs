using System.Threading.Tasks;
using TheRockPaperScissorsGame.Client.Clients;
using TheRockPaperScissorsGame.Client.Menu.Library;

namespace TheRockPaperScissorsGame.Client.Menu
{
    public class MainMenu : IMenu
    {
        private readonly UserClient _userClient;
        private readonly GameClient _gameClient;
        private readonly StatisticClient _statisticClient;

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

                MenuLibrary.Clear();

                var options = new string[] { "Authorization", "Leaderboard", "Exit" };
                var command = MenuLibrary.InputMenuItemNumber("Main Menu", options);

                switch (command)
                {
                    case 1:
                        menu = new AuthorizationMenu(_userClient, _gameClient, _statisticClient);
                        await menu.StartAsync();
                        break;
                    case 2:
                        menu = new LeaderboardMenu(_statisticClient);
                        await menu.StartAsync();
                        break;
                    case 3:
                        return;
                }
            }
        }
    }
}