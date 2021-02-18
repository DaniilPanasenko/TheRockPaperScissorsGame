using System;
using System.Threading.Tasks;

namespace TheRockPaperScissorsGame.Client.Menu
{
    public class MainMenu : IMainMenu
    {
        public MainMenu()
        {
        }

        public async Task StartAsync()
        {
            await ExecuteMainMenuAsync();
        }

        private int InputMenuItemNumber(string menuName, string[] commands)
        {
            Console.WriteLine($"\n{menuName} Menu:");
            for (int i = 0; i < commands.Length; i++)
            {
                Console.WriteLine($"{i + 1} - {commands[i]}");
            }
            Console.WriteLine("\nEnter the command number...");
            int command;
            while (!int.TryParse(Console.ReadLine(), out command) || command < 1 || command > commands.Length)
            {
                Console.WriteLine("You entered incorrect command");
                Console.WriteLine("\nEnter the command number...");
            }
            return command;
        }

        private string InputStringValue(string valueName)
        {
            Console.WriteLine($"Enter {valueName} ...");
            string value;
            value = Console.ReadLine();
            while (string.IsNullOrEmpty(value.Trim()))
            {
                Console.WriteLine($"You entered empty {valueName}");
                Console.WriteLine($"\nEnter {valueName}...");
                value = Console.ReadLine();
            }
            return value;
        }

        private async Task ExecuteMainMenuAsync()
        {
            while (true)
            {
                var options = new string[] { "Authorization", "View rate", "Exit" };
                var command = InputMenuItemNumber("Main", options);
                switch (command)
                {
                    case 1:
                        await ExecuteAuthorizationMenuAsync();
                        break;
                    case 2:
                        await ExecuteRateMenuAsync();
                        break;
                    case 3:
                    default:
                        return;
                }
            }
        }

        private async Task ExecuteAuthorizationMenuAsync()
        {
            while (true)
            {
                var options = new string[] { "Login", "Register", "Back" };
                var command = InputMenuItemNumber("Authorization", options);
                switch (command)
                {
                    case 1:
                        await ExecuteLogin();
                        break;
                    case 2:
                        await ExecuteRegistration();
                        break;
                    case 3:
                    default:
                        return;
                }
            }
        }

        private async Task ExecuteRateMenuAsync()
        {
            throw new NotImplementedException();
        }

        private async Task ExecuteLogin()
        {
            string login = InputStringValue("login");
            string password = InputStringValue("password");
            //TODO: Http request;
            //TODO: Handling response;

            //We must save the global state in some class, where we will store the token if the user is authorized

            //example (we will have status in response)
            bool success = true;
            if (success)
            {
                Console.WriteLine("Login successfully");
                await ExecuteProfileMenu();
            }
            else
            {
                //depending on the response, we will display the message
            }

        }

        private async Task ExecuteRegistration()
        {
            string login = InputStringValue("login");
            string password = InputStringValue("password");
            //TODO: Http request;
            //TODO: Handling response;

            //example (we will have status in response)
            bool success = true;
            if (success)
            {
                Console.WriteLine("Registartion successfully");
            }
            else
            {
                //depending on the response, we will display the message
            }
        }

        private async Task ExecuteProfileMenu()
        {
            while (true)
            {
                var options = new string[] { "Play", "Statistics", "Log out" };
                var command = InputMenuItemNumber("Person", options);
                switch (command)
                {
                    case 1:
                        await ExecutePlayMenu();
                        break;
                    case 2:
                        await ExecuteStatisticsMenu();
                        break;
                    case 3:
                        //we must dispose token
                    default:
                        return;
                }
            }
        }

        private Task ExecuteStatisticsMenu()
        {
            throw new NotImplementedException();
        }

        private Task ExecutePlayMenu()
        {
            throw new NotImplementedException();
        }
    }
}