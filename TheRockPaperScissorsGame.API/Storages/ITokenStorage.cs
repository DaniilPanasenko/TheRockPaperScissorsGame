using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TheRockPaperScissorsGame.API.Storages
{
    interface ITokenStorage
    {
        public string GenerateToken(string login);

        public string GetLogin(string token);
    }
}
