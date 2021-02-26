using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using TheRockPaperScissorsGame.API.Enums;
using TheRockPaperScissorsGame.API.Exceptions;
using TheRockPaperScissorsGame.API.Models;
using TheRockPaperScissorsGame.API.Storages;

namespace TheRockPaperScissorsGame.API.Services.Impl
{
    public class AuthService : IAuthService
    {
        private readonly IAccountStorage _accountStorage;

        private readonly ITokenStorage _tokenStorage;

        private readonly IUserBlockingService _userBlockingService;

        private readonly ILogger<AuthService> _logger;

        public AuthService(IAccountStorage accountStorage, ITokenStorage tokenStorage, IUserBlockingService userBlockingService, ILogger<AuthService> logger)
        {
            _accountStorage = accountStorage;
            _tokenStorage = tokenStorage;
            _userBlockingService = userBlockingService;
            _logger = logger;
        }

        public async Task<string> LoginAsync(string login, string password)
        {
            _logger.LogDebug("There we can log like this");
            var account = await _accountStorage.FindAccountAsync(login);

            if (account == null)
            {
                throw new AuthorizationException(AuthorizationStatus.IncorrectLogin);
            }
            if (_userBlockingService.IsBlocked(login))
            {
                throw new AuthorizationException(AuthorizationStatus.BlockedAccountFor1Minute);
            }
            if (account.Password != password)
            {
                _userBlockingService.NegativeLogin(login);
                throw new AuthorizationException(AuthorizationStatus.IncorrectPassword);
            }
            _userBlockingService.PositiveLogin(login);
            return _tokenStorage.GenerateToken(account.Login);
        }

        public async Task<bool> RegisterAsync(Account account)
        {
            return await _accountStorage.AddAccountAsync(account);
        }
    }
}
