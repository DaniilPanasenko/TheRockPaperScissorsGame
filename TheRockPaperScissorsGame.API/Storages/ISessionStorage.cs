using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheRockPaperScissorsGame.API.Models;

namespace TheRockPaperScissorsGame.API.Storages
{
    public interface ISessionStorage
    {
        Task AddSessionAsync(Session newSession);

        void AddToGameQuene(Session newSession);

        Task<Session> FindSessionAsync(string roomNumber);

        Session ConnectToPublicRoom(string login);

        bool ConnectToPrivateRoom(string roomNumber, string login);
    }
}
