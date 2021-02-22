using System;
using System.Threading.Tasks;
using TheRockPaperScissorsGame.API.Enums;
using TheRockPaperScissorsGame.API.Models;
using TheRockPaperScissorsGame.API.Storages;

namespace TheRockPaperScissorsGame.API.Services.Impl
{
    public class GameService : ISessionService, IRoundService
    {
        private ISessionStorage _sessionStorage;

        public GameService(ISessionStorage sessionStorage)
        {
            _sessionStorage = sessionStorage;
        }

        public Move CheckMove(string login, string id)
        {
            throw new NotImplementedException();
        }

        public async Task<string> CheckSession(string id)
        {
            var session = await _sessionStorage.FindSessionAsync(id);
            if (session == null)
            {
                throw new ArgumentNullException();
            }
            var rivalFound = session.RivalFound;
            if (!rivalFound) return null;
            else
            {
                if (session.IsBot) return "bot";
                else
                {
                    if (session.Player2Login == null)
                    {
                        throw new ArgumentNullException();
                    }
                    return session.Player2Login;
                }
            }
        }

        public void DoMove(string login, string id, Move move)
        {
            throw new NotImplementedException();
        }

        public void FinishSession(string id)
        {
            throw new NotImplementedException();
        }

        public string StartSession(string login, GameOptions options)
        {
            throw new NotImplementedException();
        }
    }
}
