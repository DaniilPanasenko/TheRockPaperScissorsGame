using System;
using System.Threading.Tasks;
using TheRockPaperScissorsGame.API.Enums;
using TheRockPaperScissorsGame.API.Exceptions;
using TheRockPaperScissorsGame.API.Models;
using TheRockPaperScissorsGame.API.Storages;

namespace TheRockPaperScissorsGame.API.Services.Impl
{
    public class SessionService : ISessionService
    {
        private ISessionStorage _sessionStorage;

        public SessionService(ISessionStorage sessionStorage)
        {
            _sessionStorage = sessionStorage;
        }

        public async Task<string> CheckSessionAsync(string roomId, string login)
        {
            var session = await _sessionStorage.FindSessionAsync(roomId);
            if (session == null)
            {
                throw new RoomConnectionException("Room not found");
            }

            if (!session.RivalFound)
            {
                return null;
            }
            else
            if (session.IsFinished)
            {
                throw new GameFinishedException(GameEndReason.RivalLeftGame);
            }
            else
            {
                if (session.IsBot) return "bot";
                else
                {
                    if (session.Player2Login == null || session.Player1Login == null)
                    {
                        throw new ArgumentNullException();
                    }
                    if (session.Player1Login == login)
                    {
                        return session.Player2Login;
                    }
                    return session.Player1Login;
                }
            }
        }

        public async Task FinishSessionAsync(string roomId)
        {
            if (roomId == null)
            {
                throw new ArgumentNullException();
            }
            var session = await _sessionStorage.FindSessionAsync(roomId);
            if (session == null)
            {
                throw new RoomConnectionException("Room not found");
            }
            session.IsFinished = true;
            session.SessionFinished = DateTime.UtcNow;
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
