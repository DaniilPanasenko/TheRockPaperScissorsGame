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

        private List<Session> _session;

        private ConcurrentQueue<Session> _connectionQueue = new ConcurrentQueue<Session>();

        private JsonWorker<Session> _jsonWorker;

        public SessionStorage(JsonWorker<Session> jsonWorker)
        {
            _jsonWorker = jsonWorker;
        }

        private async Task UploadDataAsync()
        {
                _session = await _jsonWorker.ReadDataFromFileAsync();
                _session.ForEach(session => session.IsFinished = true);
        }

        public async Task<List<Session>> GetFinishedSessions()
        {
            await _lockSlim.WaitAsync();
            try
            {
                if (_session == null)
                {
                    await UploadDataAsync();
                }

                var successfulSessions = _session.Where(session => session.IsFinished && session.Rounds.Count != 0).ToList();

                foreach (var session in successfulSessions)
                {
                    if (session.Rounds[session.Rounds.Count - 1].Player1Move == null || session.Rounds[session.Rounds.Count - 1].Player2Move == null)
                    {
                        session.Rounds.RemoveAt(session.Rounds.Count - 1);
                    }
                }

                return successfulSessions = successfulSessions.Where(session => session.Rounds.Count != 0).ToList();
            }
            finally
            {
                _lockSlim.Release();
            }
        }

        public async Task AddSessionAsync(Session newSession)
        {
            if (newSession == null)
            {
                throw new ArgumentNullException(nameof(newSession));
            }
            await _lockSlim.WaitAsync();
            try
            {
                if (_session == null)
                {
                    await UploadDataAsync();
                }
                if (_session.Any(session => session.RoomNumber == newSession.RoomNumber))
                {
                    throw new InvalidOperationException("Session with this room number is already exist");
                }

                _session.Add(newSession);
            }
            finally
            {
                _lockSlim.Release();
            }
        }
        
        public void AddToGameQueue(Session newSession)
        {
            if (newSession == null)
            {
                throw new ArgumentNullException(nameof(newSession));
            }
            if (_connectionQueue.Any(session => session.RoomNumber == newSession.RoomNumber))
            {
                 throw new InvalidOperationException("Session with this room number is already exist");
            }
            _connectionQueue.Enqueue(newSession);
        }

        public async Task<Session> FindSessionAsync(string roomNumber)
        {
            await _lockSlim.WaitAsync();
            try
            {
                if (_session == null)
                {
                    await UploadDataAsync();
                }
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

            var sessionCount = _connectionQueue.Count();
            var iteration = 0;
          
            while (_connectionQueue.TryDequeue(out Session session) && iteration!= sessionCount)
            {
                iteration++;

                if (session.IsFinished)
                    continue;

                if (session.Player1Login == login)
                {
                    _connectionQueue.Enqueue(session);
                    continue;
                }

                session.AddSecondPlayer(login);
                return session;
            }

            return null;

        }

        public async Task<bool> ConnectToPrivateRoomAsync(string roomNumber, string login)
        {
            if (roomNumber == null)
            {
                throw new ArgumentNullException();
            }

            if (login == null)
            {
                throw new ArgumentNullException();
            }

            await _lockSlim.WaitAsync();
            try
            {
                if (_session == null)
                {
                    await UploadDataAsync();
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

                session.AddSecondPlayer(login);
                return true;
            }
            finally
            {
                _lockSlim.Release();
            }
        }

        public async Task SaveSessionsAsync()
        {
            await _lockSlim.WaitAsync();
            try
            {
                if (_session == null)
                {
                    await UploadDataAsync();
                }
            }
            finally
            {
                _lockSlim.Release();
            }
            await _jsonWorker.WriteDataIntoFileAsync(await GetFinishedSessions());
        }
    }
}
