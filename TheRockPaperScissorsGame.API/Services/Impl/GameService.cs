using System;
using TheRockPaperScissorsGame.API.Enums;
using TheRockPaperScissorsGame.API.Models;

namespace TheRockPaperScissorsGame.API.Services.Impl
{
    public class GameService : ISessionService, IRoundService
    {
        public GameService()
        {
        }

        public Move CheckMove(string login, int id)
        {
            throw new NotImplementedException();
        }

        public string CheckSession(int id)
        {
            throw new NotImplementedException();
        }

        public void DoMove(string login, int id, Move move)
        {
            throw new NotImplementedException();
        }

        public void FinishSession(int id)
        {
            throw new NotImplementedException();
        }

        public string StartSession(GameOptions options)
        {
            throw new NotImplementedException();
        }
    }
}
