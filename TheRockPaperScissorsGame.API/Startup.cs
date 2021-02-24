using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using TheRockPaperScissorsGame.API.Models;
using TheRockPaperScissorsGame.API.Services;
using TheRockPaperScissorsGame.API.Services.Impl;
using TheRockPaperScissorsGame.API.Storages;
using TheRockPaperScissorsGame.API.Storages.Impl;

namespace TheRockPaperScissorsGame.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime.
        // Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IAccountStorage, AccountStorage>();
            services.AddSingleton<ITokenStorage, TokenStorage>();
            services.AddSingleton<ISessionStorage, SessionStorage>();

            services.AddSingleton<IUserBlockingService, UserBlockingService>();
            services.AddSingleton<IAuthService, AuthService>();

            services.AddSingleton(provider=>new JsonWorker<Account>("accountStorage.json"));
            services.AddSingleton(provider => new JsonWorker<Session>("sessionStorage.json"));
         
            services.AddSingleton<IRoundService, RoundService>();
            services.AddSingleton<ISessionService, SessionService>();
            services.AddSingleton<IStatisticsService, StatisticsService>();

            //services.AddSingleton<IGameService>(gameService);

            services.AddControllers().ConfigureApiBehaviorOptions(options =>
            {
                // Disable auto BadRequest response
                options.SuppressModelStateInvalidFilter = true;
                // Disable auto error explanations 
                options.SuppressMapClientErrors = true;
            });
            
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "TheRockPaperScissorsGame.API", Version = "v1" });
            });
            
        }

        // This method gets called by the runtime.
        // Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment()) app.UseDeveloperExceptionPage();

            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "TheRockPaperScissorsGame.API"));

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}