using System;
using System.Collections.Generic;
using System.Linq;
using TheRockPaperScissorsGame.API.Contracts;
using TheRockPaperScissorsGame.API.Enums;
using TheRockPaperScissorsGame.API.Models;
using TheRockPaperScissorsGame.API.Storages;

namespace TheRockPaperScissorsGame.API.Services.Impl
{
    public class StatisticsService : IStatisticsService
    {
        private ISessionStorage _sessionStorage;

        public StatisticsService(ISessionStorage sessionStorage)
        {
            _sessionStorage = sessionStorage;
        }

        public List<UserResultDto<int>> GetWinsLeaderboard(int amount)
        {
            if (amount < 0)
            {
                throw new ArgumentException();
            }

            var sessions = _sessionStorage.GetFinishedSessions();

            var users = GetUsers(sessions);

            List<UserResultDto<int>> usersResults = new List<UserResultDto<int>>();

            foreach (var user in users)
            {
                usersResults.Add(new UserResultDto<int>
                {
                    Login = user,
                    Result = GetWinsCount(sessions, user)
                });
            }

            usersResults = usersResults.OrderByDescending(result => result.Result).ToList();

            return usersResults.Take(amount).ToList();
        }

        public List<UserResultDto<TimeSpan>> GetTimeLeaderboard(int amount)
        {
            if (amount < 0)
            {
                throw new ArgumentException();
            }

            var sessions = _sessionStorage.GetFinishedSessions();

            var users = GetUsers(sessions);

            List<UserResultDto<TimeSpan>> usersResults = new List<UserResultDto<TimeSpan>>();

            foreach (var user in users)
            {
                usersResults.Add(new UserResultDto<TimeSpan>
                {
                    Login = user,
                    Result = GetUserGameTime(user)
                });
            }

            usersResults = usersResults.OrderByDescending(result => result.Result).ToList();

            return usersResults.Take(amount).ToList();
        }

        public List<UserResultDto<decimal>> GetWinsPercentLeaderboard(int amount)
        {
            if (amount < 0)
            {
                throw new ArgumentException();
            }

            var sessions = _sessionStorage.GetFinishedSessions();

            var users = GetUsers(sessions);

            List<UserResultDto<decimal>> usersResults = new List<UserResultDto<decimal>>();

            foreach (var user in users)
            {
                usersResults.Add(new UserResultDto<decimal>
                {
                    Login = user,
                    Result = GetWinsPercent(sessions, user)
                });
            }

            usersResults = usersResults.OrderByDescending(result => result.Result).ToList();

            return usersResults.Take(amount).ToList();
           
        }

        public ResultsDto GetUserResultsCount(string login)
        {
            if (login == null)
            {
                throw new ArgumentNullException();
            }

            var sessions = _sessionStorage.GetFinishedSessions();

            var winsCount = GetWinsCount(sessions, login);

            var lossCount = GetLossCount(sessions, login);

            var drawCount = GetDrawCount(sessions, login);

            return new ResultsDto
            {
                WinCount = winsCount,
                LossCount = lossCount,
                DrawCount = drawCount
            };
        }

        public TimeSpan GetUserGameTime(string login)
        {
            if (login == null)
            {
                throw new ArgumentNullException();
            }

            var sessions = _sessionStorage.GetFinishedSessions();

            TimeSpan gameTime = default;

            sessions.Where(session => session.Player1Login == login || session.Player2Login == login).ToList()
                .ForEach(session => gameTime += session.SessionFinished - session.SessionStart);

            return gameTime;
        }

        public MovesDto GetUserMovesStatistics(string login)
        {
            if (login == null)
            {
                throw new ArgumentNullException();
            }

            var sessions = _sessionStorage.GetFinishedSessions();

            MovesDto moves = new MovesDto();

            moves.RockCount = GetRockCount(sessions, login);
            moves.ScissorsCount = GetScissorsCount(sessions, login);
            moves.PaperCount = GetPaperCount(sessions, login);

            return moves;
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
            return users.Distinct().Where(user => sessions.Where(session => session.Player1Login == user || session.Player1Login == user).Select(y => y.Rounds.Count).Sum() >= 10).ToList();
        }
    }
}
