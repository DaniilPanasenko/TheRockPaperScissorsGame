using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using TheRockPaperScissorsGame.Client.Menu.Library;

namespace TheRockPaperScissorsGame.Client.Menu
{
    public class StatisticsMenu : IMenu
    {
        private readonly LeaderboardMenu _leaderboardMenu;

        private readonly PlayerStatisticsMenu _playerStatisticsMenu;

        private readonly ILogger<StatisticsMenu> _logger;

        public StatisticsMenu(LeaderboardMenu leaderboardMenu, PlayerStatisticsMenu playerStatisticsMenu, ILogger<StatisticsMenu> logger)
        {
            _leaderboardMenu = leaderboardMenu;
            _playerStatisticsMenu = playerStatisticsMenu;
            _logger = logger;
        }

        public async Task StartAsync()
        {
            _logger.LogInformation("class StatisticsMenu. StartAsync()");

            while (true)
            {
                _logger.LogInformation("Choosing the Statistics");

                MenuLibrary.Clear();

                var options = new string[] { "Leaderboard", "Player statistics", "Back" };
                var command = MenuLibrary.InputMenuItemNumber("Statistics Menu", options);

                _logger.LogInformation("Chose the command");

                switch (command)
                {
                    case 1:
                        await _leaderboardMenu.StartAsync();
                        break;
                    case 2:
                        await _playerStatisticsMenu.StartAsync();
                        break;
                    case 3:
                        return;
                }
            }
        }
    }
}