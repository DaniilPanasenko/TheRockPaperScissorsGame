using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TheRockPaperScissorsGame.API.Services
{
    internal interface IAuthService
    {
        // public string Authorize(string token);

        public Task<string> Login(string login, string password);

        public Task<bool> Register(string login, string password);
    }
}
