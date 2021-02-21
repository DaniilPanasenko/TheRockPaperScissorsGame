using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TheRockPaperScissorsGame.API.Models;

namespace TheRockPaperScissorsGame.API.Storages.Impl
{
    public class AccountStorage : IAccountStorage
    {
        static SemaphoreSlim _lockSlim = new SemaphoreSlim(1, 1);

        private List<Account> _accounts = new List<Account>();

        private JsonWorker<Account> _jsonWorker;

        private bool _isUploaded = false;

        public AccountStorage(JsonWorker<Account> jsonWorker)
        {
            _jsonWorker = jsonWorker;
        }

        public async Task<bool> AddAccountAsync(Account newAccount)
        {
            if (newAccount == null)
            {
                throw new ArgumentNullException(nameof(newAccount));
            }

            await _lockSlim.WaitAsync();
            try
            {
                if (!_isUploaded)
                {
                    await LoadData();
                }
                if (_accounts.Any(account => account.Login == newAccount.Login))
                {
                    return false;
                }

                _accounts.Add(newAccount);
                await _jsonWorker.WriteDataIntoFileAsync(_accounts);

                return true;
            }
            finally
            {
                _lockSlim.Release();
            }
        }

        public async Task<Account> FindAccountAsync(string login)
        {
            await _lockSlim.WaitAsync();
            try
            {
                if (!_isUploaded)
                {
                    await LoadData();
                }
                return _accounts.FirstOrDefault(account =>
                    account.Login == login);
            }
            finally
            {
                _lockSlim.Release();
            }
        }

        private async Task LoadData()
        {
            _accounts = await _jsonWorker.ReadDataFromFileAsync();
            _isUploaded = true;
        }
    }
}