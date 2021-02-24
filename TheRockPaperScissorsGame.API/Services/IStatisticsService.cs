using System.Collections.Generic;
using System.Threading.Tasks;
using TheRockPaperScissorsGame.API.Contracts;

namespace TheRockPaperScissorsGame.API.Services
{
    public interface IStatisticsService
    {
        Task<List<UserResultDto<int>>> GetWinsLeaderboardAsync(int amount);

        Task<List<UserResultDto<string>>> GetTimeLeaderboardAsync(int amount);

        Task<List<UserResultDto<decimal>>> GetWinsPercentLeaderboardAsync(int amount);

        Task<ResultsDto> GetUserResultsCountAsync(string login);

        Task<string> GetUserGameTimeAsync(string login);

        Task<MovesDto> GetUserMovesStatisticsAsync(string login);
    }
}
