using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using TheRockPaperScissorsGame.Client.Clients;
using TheRockPaperScissorsGame.Client.Menu;
using TheRockPaperScissorsGame.Client.Menu.Library;
using TheRockPaperScissorsGame.Client.Models;

namespace TheRockPaperScissorsGame.Client
{
    class Program
    {
        private static UserClient _userClient;
        private static GameClient _gameClient;
        private static StatisticClient _statisticClient;



        private static HttpClient GetHttpClient()
        {
            var httpClient = new HttpClient();
            var json = File.ReadAllText("clientOptions.json");
            var options = JsonSerializer.Deserialize<ClientOptions>(json);
            httpClient.BaseAddress = new Uri(options.BaseAddress);

            return httpClient;
        }

        private static IServiceProvider GetServiceProvider()
        {
            var services = new ServiceCollection();

            var httpClient = GetHttpClient();

            services.AddSingleton(provider => new UserClient(httpClient))
                .AddSingleton(provider => new GameClient(httpClient))
                .AddSingleton(provider => new StatisticClient(httpClient));

            services.AddTransient<MainMenu>()
                .AddTransient<AuthorizationMenu>()
                .AddTransient<GameMenu>()
                .AddTransient<GameStartMenu>()
                .AddTransient<IntervalResultsMenu>()
                .AddTransient<LeaderboardMenu>()
                .AddTransient<PlayerStatisticsMenu>()
                .AddTransient<StatisticsMenu>()
                .AddTransient<UserMenu>();

            var serilogLogger = new LoggerConfiguration()
           .WriteTo.File("clientLog.log")
           .CreateLogger();

            var provider = services.AddLogging(builder =>
            {
                builder.AddSerilog(logger: serilogLogger, dispose: true);
            }).BuildServiceProvider();

            return provider;

        }

        private static async Task<int> Main()
        {
            var provider = GetServiceProvider();

            while (true)
            {
                try
                {
                    MenuLibrary.Clear();
                    MenuLibrary.WriteLineColor("The Rock Paper Scissors Game. Designed by Karyna Bilotska and Daniil Panasenko\n", ConsoleColor.DarkGreen);

                    MenuLibrary.PressAnyKey();

                    GetHttpClient();
                    
                    var mainMemu = provider.GetRequiredService<MainMenu>();
                    await mainMemu.StartAsync();

                    return 0;
                }
                catch (Exception)
                {
                    ResponseLibrary.UnknownResponse();
                    MenuLibrary.PressAnyKey();
                }
            }
        }

        
    }
}