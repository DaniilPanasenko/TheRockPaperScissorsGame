using System.Threading.Tasks;
using TheRockPaperScissorsGame.Client.Clients;
using TheRockPaperScissorsGame.Client.Menu.Library;

namespace TheRockPaperScissorsGame.Client.Menu
{
    class StatisticsMenu : IMenu
    {
        private readonly StatisticClient _statisticClient;

        public StatisticsMenu(StatisticClient statisticClient)
        {
            _statisticClient = statisticClient;
        }

        public async Task StartAsync()
        {
            while (true)
            {
                MenuLibrary.Clear();
                IMenu menu;

                var options = new string[] { "Leaderboard", "Player statistics", "Back" };
                var command = MenuLibrary.InputMenuItemNumber("Statistics Menu", options);
                switch (command)
                {
                    case 1:
                        menu = new LeaderboardMenu(_statisticClient);
                        await menu.StartAsync();
                        break;
                    case 2:
                        menu = new PlayerStatisticsMenu(_statisticClient);
                        await menu.StartAsync();
                        break;
                    case 3:
                        return;
                }
            }
        }
    }
}