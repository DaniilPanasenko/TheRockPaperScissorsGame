using System;
using System.Threading.Tasks;
using TheRockPaperScissorsGame.Client.Clients;

namespace TheRockPaperScissorsGame.Client.Menu
{
    public class MainMenu : IMenu
    {
        UserClient _userClient;

        public MainMenu(UserClient userClient)
        {
            _userClient = userClient;
        }

        public async Task StartAsync()
        {
            while (true)
            {
                var options = new string[] { "Authorization", "View rate", "Exit" };
                var command = MenuLibrary.InputMenuItemNumber("Main", options);
                switch (command)
                {
                    case 1:
                        IMenu menu = new AuthorizationMenu(_userClient);
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