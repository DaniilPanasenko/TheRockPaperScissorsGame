using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheRockPaperScissorsGame.API.Contracts;
using TheRockPaperScissorsGame.API.Enums;
using TheRockPaperScissorsGame.API.Models;
using TheRockPaperScissorsGame.API.Storages;

namespace TheRockPaperScissorsGame.API.Services.Impl
{
    public class StatisticsService : IStatisticsService
    {
        private readonly ISessionStorage _sessionStorage;

        private readonly ILogger<StatisticsService> _logger;

        public StatisticsService(ISessionStorage sessionStorage, ILogger<StatisticsService> logger)
        {
            _sessionStorage = sessionStorage;
            _logger = logger;
        }

        public async Task<List<UserResultDto>> GetWinsLeaderboardAsync(int amount)
        {
            _logger.LogDebug("");

            if (amount < 0)
            {
                throw new ArgumentException();
            }

            var sessions = await _sessionStorage.GetFinishedSessionsAsync();

            var users = GetUsers(sessions);

            List<UserResultDto> usersResults = new List<UserResultDto>();

            foreach (var user in users)
            {
                usersResults.Add(new UserResultDto
                {
                    Login = user,
                    Result = GetWinsCount(sessions, user).ToString()
                });
            }

            usersResults = usersResults.OrderByDescending(result => int.Parse(result.Result)).ToList();

            return usersResults.Take(amount).ToList();
        }

        public async Task<List<UserResultDto>> GetTimeLeaderboardAsync(int amount)
        {
            if (amount < 0)
            {
                throw new ArgumentException();
            }

            var sessions = await _sessionStorage.GetFinishedSessionsAsync();

            var users = GetUsers(sessions);

            List<UserResultDto> usersResults = new List<UserResultDto>();

            foreach (var user in users)
            {
                usersResults.Add(new UserResultDto
                {
                    Login = user,
                    Result = await GetUserGameTimeAsync(user)
                });
            }

            usersResults = usersResults.OrderByDescending(result => TimeSpan.Parse(result.Result)).ToList();

            return usersResults.Take(amount).ToList();
        }

        public async Task<List<UserResultDto>> GetWinsPercentLeaderboardAsync(int amount)
        {
            if (amount < 0)
            {
                throw new ArgumentException();
            }

            var sessions = await _sessionStorage.GetFinishedSessionsAsync();

            var users = GetUsers(sessions);

            List<UserResultDto> usersResults = new List<UserResultDto>();

            foreach (var user in users)
            {
                usersResults.Add(new UserResultDto
                {
                    Login = user,
                    Result = GetWinsPercent(sessions, user).ToString()
                });
            }

            usersResults = usersResults.OrderByDescending(result => decimal.Parse(result.Result)).ToList();

            return usersResults.Take(amount).ToList();
           
        }

        public async Task<ResultsDto> GetUserResultsCountAsync(string login)
        {
            if (login == null)
            {
                throw new ArgumentNullException();
            }

            var sessions = await _sessionStorage.GetFinishedSessionsAsync();
            ResultsDto results = new ResultsDto
            {
                WinCount = GetWinsCount(sessions, login),
                DrawCount = GetDrawCount(sessions, login),
                LossCount = GetLossCount(sessions, login)
            };
          
            return results;
        }

        public async Task<string> GetUserGameTimeAsync(string login)
        {
            if (login == null)
            {
                throw new ArgumentNullException();
            }

            var sessions = await _sessionStorage.GetFinishedSessionsAsync();

            TimeSpan gameTime = default;

            sessions.Where(session => session.Player1Login == login || session.Player2Login == login).ToList()
                .ForEach(session => gameTime += session.SessionFinished - session.SessionStart);

            return gameTime.ToString("dd':'hh':'mm':'ss");
        }

        public async Task<MovesDto> GetUserMovesStatisticsAsync(string login)
        {
            if (login == null)
            {
                throw new ArgumentNullException();
            }

            var sessions = await _sessionStorage.GetFinishedSessionsAsync();


            MovesDto moves = new MovesDto
            {
                RockCount = GetRockCount(sessions, login),
                ScissorsCount = GetScissorsCount(sessions, login),
                PaperCount = GetPaperCount(sessions, login)
            };

            return moves;
        }

        public async Task<List<ResultsByTimeDto>> GetResultsByTimeAsync(string login, int amount, TimeInterval timeInterval)
        {
            if (login == null)
            {
                throw new ArgumentNullException();
            }

            var sessions = await _sessionStorage.GetFinishedSessionsAsync();
            sessions = sessions.Where(x => x.Player1Login == login || x.Player2Login == login).OrderByDescending(x=>x.SessionStart).ToList();

            var results = new List<ResultsByTimeDto>();

            if (sessions.Count == 0)
            {
                return results;
            }

            var firstGame = sessions.Last().SessionStart;

            TimeSpan interval = new TimeSpan(0);
            switch (timeInterval)
            {
                case TimeInterval.Day:
                    interval = TimeSpan.FromDays(1);
                    break;
                case TimeInterval.Hour:
                    interval = TimeSpan.FromHours(1);
                    break;
                case TimeInterval.Minute:
                    interval = TimeSpan.FromMinutes(1);
                    break;
            }

            var fromTime = new DateTime(DateTime.UtcNow.Ticks + 1000 - DateTime.UtcNow.Ticks % interval.Ticks);
            results.Add(new ResultsByTimeDto(fromTime.ToString()));
            foreach(var session in sessions)
            {
                if (session.SessionStart < fromTime)
                {
                    if (results.Count == amount) break;
                    fromTime = new DateTime(fromTime.Ticks - interval.Ticks);
                    results.Add(new ResultsByTimeDto(fromTime.ToString()));
                }
                var isFirst = login == session.Player1Login;
                results.Last().WinCount += session.Rounds
                    .Where(x =>
                        (x.WinType == WinType.FirstPlayer && isFirst) ||
                        (x.WinType == WinType.SecondPlayer && !isFirst))
                    .Count();
                results.Last().LossCount += session.Rounds
                    .Where(x =>
                        (x.WinType == WinType.FirstPlayer && !isFirst) ||
                        (x.WinType == WinType.SecondPlayer && isFirst))
                    .Count();
            }
            return results;
        }

        private int GetWinsCount(List<Session> sessions, string login)
        {
            return sessions.Where(x => x.Player1Login == login || x.Player2Login == login)
                       .Sum(x => x.Rounds.Where(y => (x.Player1Login == login && y.WinType == WinType.FirstPlayer) ||
                                                     (x.Player2Login == login && y.WinType == WinType.SecondPlayer)).Count());
        }

        private int GetLossCount(List<Session> sessions, string login)
        {
            return sessions.Where(x => x.Player1Login == login || x.Player2Login == login)
                   .Sum(x => x.Rounds.Where(y => (x.Player1Login == login && y.WinType == WinType.SecondPlayer) ||
                                                 (x.Player2Login == login && y.WinType == WinType.FirstPlayer)).Count());
        }

        private int GetDrawCount(List<Session> sessions, string login)
        {
            return sessions.Where(x => x.Player1Login == login || x.Player2Login == login)
                    .Sum(x => x.Rounds.Where(y => ((x.Player1Login == login || x.Player2Login == login) && y.WinType == WinType.Draw)).Count());
        }

        private decimal GetWinsPercent(List<Session> sessions, string login)
        {
            var wins = GetWinsCount(sessions, login);
            var total = wins + GetDrawCount(sessions, login) + GetLossCount(sessions, login);

            return Math.Round(((decimal)wins / (decimal)total) * 100, 2);
        }

        private int GetRockCount(List<Session> sessions, string login)
        {
            return sessions.Where(x => x.Player1Login == login || x.Player2Login == login)
                       .Sum(x => x.Rounds.Where(y => (x.Player1Login == login && y.Player1Move == Move.Rock) ||
                                                     (x.Player2Login == login && y.Player2Move == Move.Rock)).Count());
        }

        private int GetPaperCount(List<Session> sessions, string login)
        {
            return sessions.Where(x => x.Player1Login == login || x.Player2Login == login)
                       .Sum(x => x.Rounds.Where(y => (x.Player1Login == login && y.Player1Move == Move.Paper) ||
                                                     (x.Player2Login == login && y.Player2Move == Move.Paper)).Count());
        }

        private int GetScissorsCount(List<Session> sessions, string login)
        {
            return sessions.Where(x => x.Player1Login == login || x.Player2Login == login)
                       .Sum(x => x.Rounds.Where(y => (x.Player1Login == login && y.Player1Move == Move.Scissors) ||
                                                     (x.Player2Login == login && y.Player2Move == Move.Scissors)).Count());
        }

        private List<string> GetUsers(List<Session> sessions)
        {
            var users = sessions.Select(session => session.Player1Login).ToList();
            users.AddRange(sessions.Where(session => session.Player2Login != null).Select(session => session.Player2Login).ToList());

            return users.Distinct().Where(user => sessions.Where(session => session.Player1Login == user || session.Player2Login == user).Select(y => y.Rounds.Count).Sum() >= 10).ToList();
        }
    }
}