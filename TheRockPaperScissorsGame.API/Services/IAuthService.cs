using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheRockPaperScissorsGame.API.Models;

namespace TheRockPaperScissorsGame.API.Services
{
  public  interface IAuthService
    {
        public Task<string> LoginAsync(string login, string password);

        public Task<bool> RegisterAsync(Account account);
    }
}
