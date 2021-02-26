using Microsoft.Extensions.Logging;
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

        private readonly JsonWorker<Account> _jsonWorker;

        private readonly ILogger<AccountStorage> _logger;

        private bool _isUploaded = false;

        public AccountStorage(JsonWorker<Account> jsonWorker, ILogger<AccountStorage> logger)
        {
            _jsonWorker = jsonWorker;
            _logger = logger;
        }

        public async Task<bool> AddAccountAsync(Account newAccount)
        {
            _logger.LogDebug("");

            if (newAccount == null)
            {
                throw new ArgumentNullException(nameof(newAccount));
            }

            await _lockSlim.WaitAsync();
            try
            {
                if (!_isUploaded)
                {
                    await LoadDataAsync();
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
                    await LoadDataAsync();
                }
                return _accounts.FirstOrDefault(account =>
                    account.Login == login);
            }
            finally
            {
                _lockSlim.Release();
            }
        }

        private async Task LoadDataAsync()
        {
            _accounts = await _jsonWorker.ReadDataFromFileAsync();
            _isUploaded = true;
        }
    }
}