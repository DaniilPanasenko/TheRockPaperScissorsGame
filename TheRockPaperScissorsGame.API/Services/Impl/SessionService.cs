using Microsoft.Extensions.Logging;
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
        private readonly ISessionStorage _sessionStorage;

        private readonly ILogger<SessionService> _logger;

        public SessionService(ISessionStorage sessionStorage, ILogger<SessionService> logger)
        {
            _sessionStorage = sessionStorage;
            _logger = logger;
        }

        public async Task<string> CheckSessionAsync(string roomId, string login)
        {
            _logger.LogDebug("");
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
                if (session.IsBot)
                {
                    return "bot";
                }
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
                session.AddSecondPlayer(login);
                return session.RoomNumber;
            }
        }
    }
}