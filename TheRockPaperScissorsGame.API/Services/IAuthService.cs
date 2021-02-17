using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TheRockPaperScissorsGame.API.Services
{
    interface IAuthService
    {
        // public string Authorize(string token);

        public string Login(string login, string password);

        public bool Register(string login, string password);
    }
}
