using System;
using TheRockPaperScissorsGame.API.Models;

namespace TheRockPaperScissorsGame.API.Services
{
    public interface ISessionService
    {
        public string StartSession(GameOptions options);

        public string CheckSession(int id);

        public void FinishSession(int id);
    }
}
