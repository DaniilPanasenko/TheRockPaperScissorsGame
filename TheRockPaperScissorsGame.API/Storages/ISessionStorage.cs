using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheRockPaperScissorsGame.API.Models;

namespace TheRockPaperScissorsGame.API.Storages
{
    interface ISessionStorage
    {
        Task<bool> AddSessionAsync(Session newSession);

        Task<Session> FindSessionAsync(string roomNumber);
    }
}
