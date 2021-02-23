using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheRockPaperScissorsGame.API.Enums;
using TheRockPaperScissorsGame.API.Exceptions;
using TheRockPaperScissorsGame.API.Storages;

namespace TheRockPaperScissorsGame.API.Services.Impl
{
    public class RoundService: IRoundService
    {
        private ISessionStorage _sessionStorage;

        public RoundService(ISessionStorage sessionStorage)
        {
            _sessionStorage = sessionStorage;
        }

        public async Task DoMoveAsync(string login, string id, Move move)
        {
            if (login == null)
            {
                throw new ArgumentNullException();
            }
            if (id == null)
            {
                throw new ArgumentNullException();
            }

            var session = await _sessionStorage.FindSessionAsync(id);

            if (session == null)
            {
                throw new RoomConnectionException("Room was not found");
            }

            if (session.Player1Login != login && session.Player2Login != login)
            {
                throw new RoomConnectionException("User does not exist");
            }

            var isFirst = session.Player1Login == login;

            await session.AddMoveAsync(isFirst, move);

            if (session.IsBot)
            {
                Random rnd = new Random();
               
                await session.AddMoveAsync(false, (Move)rnd.Next(0, 3));
            }
        }

        public async Task <Move?> CheckMoveAsync(string login, string id)
        {
            if (login == null)
            {
                throw new ArgumentNullException();
            }
            if (id == null)
            {
                throw new ArgumentNullException();
            }

            var session = await _sessionStorage.FindSessionAsync(id);

            if (session == null)
            {
                throw new RoomConnectionException("Room was not found");
            }

            if (session.Player1Login != login && session.Player2Login != login)
            {
                throw new RoomConnectionException("User does not exist");
            }

            var isFirst = session.Player1Login == login;

            return await session.GetMoveAsync(isFirst);
        }
    }
}
