using System.Threading.Tasks;
using TheRockPaperScissorsGame.API.Models;

namespace TheRockPaperScissorsGame.API.Services
{
    public interface ISessionService
    {
        public Task<string> StartSessionAsync(string login, GameOptions options);

        public Task<string> CheckSessionAsync(string id,string login);

        public Task FinishSessionAsync(string id);
    }
}
