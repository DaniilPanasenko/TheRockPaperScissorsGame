using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using TheRockPaperScissorsGame.Client.Menu.Library;

namespace TheRockPaperScissorsGame.Client.Menu
{
    public class UserMenu : IMenu
    {
        private readonly GameStartMenu _gameStartMenu;

        private readonly StatisticsMenu _statisticsMenu;

        private readonly ILogger<UserMenu> _logger;

        public UserMenu(GameStartMenu gameStartMenu, StatisticsMenu statisticsMenu, ILogger<UserMenu> logger)
        {
            _gameStartMenu = gameStartMenu;
            _statisticsMenu = statisticsMenu;
            _logger = logger;
        }

        public async Task StartAsync()
        {
            _logger.LogInformation("class UserMenu. StartAsync()");

            while (true)
            {
                _logger.LogInformation("Choosing the option");

                MenuLibrary.Clear();

                var options = new string[] { "Game", "Statistics", "Logout" };
                var command = MenuLibrary.InputMenuItemNumber("User Menu", options);

                _logger.LogInformation("Chose the command");

                switch (command)
                {
                    case 1:
                        await _gameStartMenu.StartAsync();
                        break;
                    case 2:
                        await _statisticsMenu.StartAsync();
                        break;
                    case 3:
                        return;
                }
            }
        }
    }
}