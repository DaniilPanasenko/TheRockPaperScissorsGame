using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheRockPaperScissorsGame.API.Models;
using TheRockPaperScissorsGame.API.Storages;

namespace TheRockPaperScissorsGame.API.Services.Impl
{
    class AuthService : IAuthService
    {
        private readonly IAccountStorage _accountStorage;
        private readonly ITokenStorage _tokenStorage;
        // one more dependency?
       
        public AuthService(IAccountStorage accountStorage, ITokenStorage tokenStorage)
        {
            _accountStorage = accountStorage;
            _tokenStorage = tokenStorage;
        }

        // Hmm.. Is it should work something like this?

        //public string Authorize(string login) // (string token)
        //{
        //    // In the Solid project was done the next way:
        //    //if (token == null || !_tokenStorage.ContainsKey(token)) return null;
        //    //return _tokens[token];
        //    //but actually we need to do next

        //    //_tokenStorage.GenerateToken(login);
        //}

        // TODO: finish it
        public string Login(string login, string password)
        {
            // Blocking and other stuff we need to add

            var account = _accountStorage.FindAccount(login, password);
            if (account == null) return null;

            return _tokenStorage.GenerateToken(account.Login);

        }

        public bool Register(string login, string password)
        {
            return _accountStorage.AddAccount(new Account
            {
                Login = login,
                Password = password
            });
        }
    }
}
