using System.Collections.Generic;
using System.Threading.Tasks;
using TheRockPaperScissorsGame.API.Contracts;
using TheRockPaperScissorsGame.API.Enums;

namespace TheRockPaperScissorsGame.API.Services
{
    public interface IStatisticsService
    {
        Task<List<UserResultDto>> GetWinsLeaderboardAsync(int amount);

        Task<List<UserResultDto>> GetTimeLeaderboardAsync(int amount);

        Task<List<UserResultDto>> GetWinsPercentLeaderboardAsync(int amount);

        Task<ResultsDto> GetUserResultsCountAsync(string login);

        Task<string> GetUserGameTimeAsync(string login);

        Task<MovesDto> GetUserMovesStatisticsAsync(string login);

        Task<List<ResultsByTimeDto>> GetResultsByTimeAsync(string login, int amount, TimeInterval timeInterval);
    }
}
