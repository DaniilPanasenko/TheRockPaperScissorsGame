using System;
using System.Collections.Generic;
using System.Linq;
using TheRockPaperScissorsGame.API.Models;

namespace TheRockPaperScissorsGame.API.Storages.Impl
{
    public class AccountStorage: IAccountStorage
    {
        private readonly List<Account> _accounts = new List<Account>();

        public AccountStorage()
        {
        }

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

        // As we have already metioned, login will be as unique id, so I check login
        public bool AddAccount(Account newAccount)
        {
            if (newAccount == null)
            { 
                throw new ArgumentNullException(nameof(newAccount)); 
            }

            if (_accounts.Any(account => account.Login == newAccount.Login))
            {
                return false;
            }

            _accounts.Add(newAccount);
            return true;
        }

        public Account FindAccount(string login, string password)
        {
            return _accounts.FirstOrDefault(account =>
                account.Login == login &&
                account.Password == password);
        }
    }
}
