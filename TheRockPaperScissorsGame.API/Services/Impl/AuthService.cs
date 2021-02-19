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
                throw new InvalidCastException();//TODO: add exception
            }
            return _tokenStorage.GenerateToken(account.Login);
        }

        public async Task<bool> Register(Account account)
        {
            return await _accountStorage.AddAccountAsync(account);
        }
    }
}
