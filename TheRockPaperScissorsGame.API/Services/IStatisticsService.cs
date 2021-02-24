using System;
using System.Collections.Generic;
using TheRockPaperScissorsGame.API.Contracts;

namespace TheRockPaperScissorsGame.API.Services
{
    public interface IStatisticsService
    {
        List<UserResultDto<int>> GetWinsLeaderboard(int amount);

        List<UserResultDto<TimeSpan>> GetTimeLeaderboard(int amount);

        List<UserResultDto<decimal>> GetWinsPercentLeaderboard(int amount);

        ResultsDto GetUserResultsCount(string login);

        TimeSpan GetUserGameTime(string login);

        MovesDto GetUserMovesStatistics(string login);
    }
}
