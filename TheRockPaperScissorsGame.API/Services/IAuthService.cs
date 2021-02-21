using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheRockPaperScissorsGame.API.Models;

namespace TheRockPaperScissorsGame.API.Services
{
  public  interface IAuthService
    {
        // public string Authorize(string token);

        public Task<string> Login(string login, string password);

        public Task<bool> Register(Account account);
    }
}
