using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TheRockPaperScissorsGame.API.Models;

namespace TheRockPaperScissorsGame.API.Storages.Impl
{
   public class SessionStorage : ISessionStorage
   {
        static SemaphoreSlim _lockSlim = new SemaphoreSlim(1, 1);

        private List<Session> _session = null;

        private ConcurrentQueue<Session> _connectionQueue = new ConcurrentQueue<Session>();

        private JsonWorker<Session> _jsonWorker;

        public SessionStorage(JsonWorker<Session> jsonWorker)
        {
            _jsonWorker = jsonWorker;
        }

        // changed to Task, was Task<bool>?
        public async Task AddSessionAsync(Session newSession)
        {
            if (newSession == null)
            {
                throw new ArgumentNullException(nameof(newSession));
            }

            await _lockSlim.WaitAsync();
            try
            {
                if (_session.Any(session => session.RoomNumber == newSession.RoomNumber))
                {
                    // throw an exeption
                }

                _session.Add(newSession);

                // we probably don't need that...
                // await _jsonWorker.WriteDataIntoFileAsync(_session);
            }
            finally
            {
                _lockSlim.Release();
            }
        }
        
        // changed to void, was bool
        public void AddToGameQuene(Session newSession)
        {
            if (newSession == null)
            {
                throw new ArgumentNullException(nameof(newSession));
            }

            if (_connectionQueue.Any(session => session.RoomNumber == newSession.RoomNumber))
            {
                // Throw execption
            }
            _connectionQueue.Enqueue(newSession);
        }

        public async Task<Session> FindSessionAsync(string roomNumber)
        {
            await _lockSlim.WaitAsync();
            try
            {
                return _session.FirstOrDefault(session =>
                    session.RoomNumber == roomNumber);
            }
            finally
            {
                _lockSlim.Release();
            }
        }

        public Session ConnectToPublicRoom(string login)
        {
            if (login == null)
            {
                throw new ArgumentNullException();
            }

            if (_connectionQueue.TryDequeue(out Session session))
            {
                session.Player2Login = login;
                return session;
            }

            return null;
        }

        public bool ConnectToPrivateRoom(string roomNumber, string login)
        {
            if (roomNumber == null)
            {
                throw new ArgumentNullException();
            }

            if (login == null)
            {
                throw new ArgumentNullException();
            }

            var session = _session.FirstOrDefault(session =>
                    session.RoomNumber == roomNumber);

            if (session==null)
            {
                return false;
            }

            if (session.IsBot || session.Player2Login != null)
            {
                return false;
            }

            session.Player2Login = login;

            return true;

        }

        private async Task SaveSessions()
        {
            var successfulSessins = _session.Where(session => session.IsFinished && session.Rounds.Count != 0).ToList();
            await _jsonWorker.WriteDataIntoFileAsync(successfulSessins);
        }
    }
}
