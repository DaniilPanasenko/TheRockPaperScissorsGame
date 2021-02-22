using System;
using System.Threading.Tasks;
using TheRockPaperScissorsGame.API.Enums;
using TheRockPaperScissorsGame.API.Exceptions;
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

        public async Task<string> CheckSessionAsync(string id)
        {
            var session = await _sessionStorage.FindSessionAsync(id);
            if (session == null)
            {
                throw new RoomConnectionException("Room not found");
            }

            if (!session.RivalFound)
            {
                return null;
            }
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

        public async Task FinishSessionAsync(string id)
        {
            if (id != null)
            {
                throw new ArgumentNullException();
            }
            var session = await _sessionStorage.FindSessionAsync(id);
            if (session == null)
            {
                throw new RoomConnectionException("Room not found");
            }
            session.IsFinished = true;
            await _sessionStorage.SaveSessionsAsync();
        }

        public async Task<string> StartSessionAsync(string login, GameOptions options)
        {
            if (login == null)
            {
                throw new ArgumentNullException();
            }
            if (options == null)
            {
                throw new ArgumentNullException();
            }

            if (options.RoomType == RoomType.Public)
            {
                var session  = _sessionStorage.ConnectToPublicRoom(login);

                if (session == null)
                {
                    var newSession = new Session(options, login);
                    await _sessionStorage.AddSessionAsync(newSession);
                    _sessionStorage.AddToGameQueue(newSession);
                    return newSession.RoomNumber;
                }

                return session.RoomNumber;
            }
            else if(options.RoomType!=RoomType.Private || options.RoomNumber==null)
            {
                var newSession = new Session(options, login);
                await _sessionStorage.AddSessionAsync(newSession);
                return newSession.RoomNumber;
            }
            else
            {
                var session = await _sessionStorage.FindSessionAsync(options.RoomNumber);
                if (session == null)
                {
                    throw new RoomConnectionException("Room not found");
                }
                if (session.IsBot || session.Player2Login != null)
                {
                    throw new RoomConnectionException("Room is occupated");
                }
                session.Player2Login = login;
                return session.RoomNumber;
            }
        }
    }
}
