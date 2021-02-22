using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheRockPaperScissorsGame.API.Enums;
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

        public void DoMove(string login, string id, Move move)
        {
            throw new NotImplementedException();
        }

        public Move CheckMove(string login, string id)
        {
            throw new NotImplementedException();
        }
    }
}
