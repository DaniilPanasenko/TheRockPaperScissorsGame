using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using TheRockPaperScissorsGame.API.Models;

namespace TheRockPaperScissorsGame.API.Services.Impl
{
    public class UserBlockingService : IUserBlockingService
    {
        private List<UserLoginAttempts> _userLoginAttemptsList;

        private readonly ILogger<UserBlockingService> _logger;

        private readonly object locker = new object();

        public UserBlockingService(ILogger<UserBlockingService> logger)
        {
            _userLoginAttemptsList = new List<UserLoginAttempts>();
            _logger = logger;
        }

        public void NegativeLogin(string login)
        {
            _logger.LogDebug("");

            lock (locker)
            {
                var userLoginAttempts = _userLoginAttemptsList
                    .Where(x => x.UserLogin == login)
                    .FirstOrDefault();

                if (userLoginAttempts == null)
                {
                    _userLoginAttemptsList.Add(new UserLoginAttempts(login));
                }
                else
                {
                    userLoginAttempts.AddAttempt();
                }
            }
        }

        public bool IsBlocked(string login)
        {
            lock (locker)
            {
                if (_userLoginAttemptsList.Count == 0)
                {
                    return false;
                }
                else
                {
                    var userLoginAttempts = _userLoginAttemptsList
                        .Where(x => x.UserLogin == login)
                        .FirstOrDefault();

                    return userLoginAttempts == null ? false : userLoginAttempts.IsBlocked;
                }
            }
        }

        public void PositiveLogin(string login)
        {
            lock (locker)
            {
                _userLoginAttemptsList.RemoveAll(x => x.UserLogin == login);
            }
        }
    }
}
