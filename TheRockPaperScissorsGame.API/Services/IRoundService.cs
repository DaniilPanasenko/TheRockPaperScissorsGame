using System;
namespace TheRockPaperScissorsGame.API.Services
{
    public interface IRoundService
    {
        public void DoMove(string login, int id, Move move);

        public Move CheckMove(string login, int id);
    }
}
