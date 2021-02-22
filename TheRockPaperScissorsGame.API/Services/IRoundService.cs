using System;
using TheRockPaperScissorsGame.API.Enums;

namespace TheRockPaperScissorsGame.API.Services
{
    public interface IRoundService
    {
        public void DoMove(string login, string id, Move move);

        public Move CheckMove(string login, string id);
    }
}
