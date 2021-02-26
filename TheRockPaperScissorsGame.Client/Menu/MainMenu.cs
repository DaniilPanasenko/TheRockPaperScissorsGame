using System.Threading.Tasks;
using TheRockPaperScissorsGame.Client.Clients;
using TheRockPaperScissorsGame.Client.Menu.Library;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace TheRockPaperScissorsGame.Client.Menu
{
    public class MainMenu : IMenu
    {
        private readonly ILogger<MainMenu> _logger;
        private readonly LeaderboardMenu _leaderMenu;
        private readonly AuthorizationMenu _authMenu;

        public MainMenu(AuthorizationMenu authMenu, LeaderboardMenu leaderMenu, ILogger<MainMenu> logger)
        {
            _logger = logger;
            _leaderMenu = leaderMenu;
            _authMenu = authMenu;
        }

        public async Task StartAsync()
        {
            _logger.LogInformation("In the MainMenu");

            while (true)
            {
                MenuLibrary.Clear();

                _logger.LogInformation("Choosing the command");

                var options = new string[] { "Authorization", "Leaderboard", "Exit" };
                var command = MenuLibrary.InputMenuItemNumber("Main Menu", options);

                _logger.LogInformation("Chose the command");


                switch (command)
                {
                    case 1:
                        await _authMenu.StartAsync();
                        break;
                    case 2:
                        await _leaderMenu.StartAsync();
                        break;
                    case 3:
                        return;
                }
            }
        }
    }
}