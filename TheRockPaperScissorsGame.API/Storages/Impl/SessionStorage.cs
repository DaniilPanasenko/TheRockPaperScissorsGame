using System;
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

        private JsonWorker<Session> _jsonWorker;

        private bool _isUploaded = false;

        public SessionStorage(JsonWorker<Session> jsonWorker)
        {
            _jsonWorker = jsonWorker;
        }

        public async Task<bool> AddSessionAsync(Session newSession)
        {
            if (newSession == null)
            {
                throw new ArgumentNullException(nameof(newSession));
            }

            await _lockSlim.WaitAsync();
            try
            {
                if (!_isUploaded)
                {
                    await LoadData();
                }

                if (_session.Any(session => session.RoomNumber == newSession.RoomNumber))
                {
                    return false;
                }

                _session.Add(newSession);
                await _jsonWorker.WriteDataIntoFileAsync(_session);

                return true;
            }
            finally
            {
                _lockSlim.Release();
            }
        }

        public async Task<Session> FindSessionAsync(string roomNumber)
        {
            await _lockSlim.WaitAsync();
            try
            {
                if (!_isUploaded)
                {
                    await LoadData();
                }
                return _session.FirstOrDefault(session =>
                    session.RoomNumber == roomNumber);
            }
            finally
            {
                _lockSlim.Release();
            }
        }

        private async Task LoadData()
        {
            _session = await _jsonWorker.ReadDataFromFileAsync();
            _isUploaded = true;
        }
    }
}
