using System;
using System.Threading.Tasks;
using TheRockPaperScissorsGame.Client.Menu;

namespace TheRockPaperScissorsGame.Client
{
    class Program
    {
        private static async Task<int> Main()
        {
            try
            {
                Console.WriteLine("The Rock Paper Scissors Game. Designed by Karyna Bilotska and Daniil Panasenko\n");
                IMainMenu mainMenu = new MainMenu();
                await mainMenu.StartAsync();
                return 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Critical unhandled exception");
                Console.WriteLine(ex);
                return -1;
            }
        }
    }
}
