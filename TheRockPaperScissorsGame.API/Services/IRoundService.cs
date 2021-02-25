using System;
using System.Threading.Tasks;
using TheRockPaperScissorsGame.API.Contracts;
using TheRockPaperScissorsGame.API.Enums;

namespace TheRockPaperScissorsGame.API.Services
{
    public interface IRoundService
    {
        public Task DoMoveAsync(string login, string id, Move move);

        public Task<RoundResultDto> CheckMoveAsync(string login, string id);
    }
}
