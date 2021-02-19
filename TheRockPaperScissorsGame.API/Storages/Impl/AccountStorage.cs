using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheRockPaperScissorsGame.API.Models;

namespace TheRockPaperScissorsGame.API.Storages.Impl
{
    internal class AccountStorage: IAccountStorage
    {
        //we must do it cuncurency
        private List<Account> _accounts = new List<Account>();

        private JsonWorker<Account> _jsonWorker;

        private bool _isUploaded = false;

        public AccountStorage(JsonWorker<Account> jsonWorker)
        {
            _jsonWorker = jsonWorker;
        }

        //Why do we need this method?
        /*
        public bool Register(string login, string password)
        {
            if (_accounts.Any(account => account.Login == login)) return false;

            _accounts.Add(new Account
            {
                Login = login,
                Password = password
            });

            return true;
        }
        */

        // As we have already metioned, login will be as unique id, so I check login
        //json worker : will be async
        public async Task<bool> AddAccountAsync(Account newAccount)
        {
            if (newAccount == null)
            { 
                throw new ArgumentNullException(nameof(newAccount)); 
            }
            if (!_isUploaded)
            {
                _accounts = await _jsonWorker.ReadDataFromFileAsync();
                _isUploaded = true;
            }
            if (_accounts.Any(account => account.Login == newAccount.Login))
            {
                return false;
            }
            _accounts.Add(newAccount);
            await _jsonWorker.WriteDataIntoFileAsync(_accounts);
            return true;
        }

        public async Task<Account> FindAccountAsync(string login)
        {
            if (!_isUploaded)
            {
                _accounts = await _jsonWorker.ReadDataFromFileAsync();
                _isUploaded = true;
            }
            return _accounts.FirstOrDefault(account =>
                account.Login == login);
        }
    }
}
