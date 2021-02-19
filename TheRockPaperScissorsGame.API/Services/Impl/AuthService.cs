using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheRockPaperScissorsGame.API.Models;
using TheRockPaperScissorsGame.API.Storages;

namespace TheRockPaperScissorsGame.API.Services.Impl
{
    internal class AuthService : IAuthService
    {
        private readonly IAccountStorage _accountStorage;
        private readonly ITokenStorage _tokenStorage;
        private readonly IUserBlockingService _userBlockingService;
        // one more dependency?
       
        public AuthService(IAccountStorage accountStorage, ITokenStorage tokenStorage, IUserBlockingService userBlockingService)
        {
            _accountStorage = accountStorage;
            _tokenStorage = tokenStorage;
            _userBlockingService = userBlockingService;
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
        /*
        public async Task<string> Login(string login, string password)
        {
            var account = await _accountStorage.FindAccountAsync(login, password);
            if (account == null)
            {
                var accountByLogin = await _accountStorage.FindAccountAsync(login);
                if (accountByLogin != null)
                {
                    _userBlockingService.NegativeLogin(login);
                }
                return null;
            }
            if (!_userBlockingService.TryPositiveLogin(login))
            {
                throw new Exception();
            }
            return _tokenStorage.GenerateToken(account.Login);
        }
        */
        public async Task<string> Login(string login, string password)
        {
            var account = await _accountStorage.FindAccountAsync(login);

            if (account == null) return null;
            if (account.Password != password)
            {
                _userBlockingService.NegativeLogin(login);
                return null;
            }
            if (!_userBlockingService.TryPositiveLogin(login))
            {
                throw new Exception();//TODO: add exception
            }
            return _tokenStorage.GenerateToken(account.Login);
        }

        public async Task<bool> Register(string login, string password)
        {
            return await _accountStorage.AddAccountAsync(new Account
            {
                Login = login,
                Password = password
            });
        }
    }
}
