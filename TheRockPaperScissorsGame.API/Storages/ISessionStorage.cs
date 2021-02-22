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

        void AddToGameQueue(Session newSession);

        Task<Session> FindSessionAsync(string roomNumber);

        Session ConnectToPublicRoom(string login);

        Task<bool> ConnectToPrivateRoomAsync(string roomNumber, string login);

        Task SaveSessionsAsync();
    }
}
