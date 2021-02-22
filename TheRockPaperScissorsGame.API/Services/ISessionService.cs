using System;
using System.Threading.Tasks;
using TheRockPaperScissorsGame.API.Models;

namespace TheRockPaperScissorsGame.API.Services
{
    public interface ISessionService
    {
        public string StartSession(string login, GameOptions options);

        public Task<string> CheckSession(string id);

        public void FinishSession(string id);
    }
}
