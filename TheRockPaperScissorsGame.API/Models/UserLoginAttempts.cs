using System;

namespace TheRockPaperScissorsGame.API.Models
{
    public class UserLoginAttempts
    {
        private TimeSpan _blockingTime = TimeSpan.FromMinutes(1);

        public UserLoginAttempts(string login)
        {
            UserLogin = login;
            AttemptsCount = 1;
            LastAttempt = DateTime.UtcNow;
        }

        public string UserLogin { get; private set; }

        public int AttemptsCount { get; private set; }

        public DateTime LastAttempt { get; private set; }

        public bool IsBlocked => AttemptsCount >= 3 && DateTime.UtcNow - LastAttempt < _blockingTime;


        public void AddAttempt()
        {
            AttemptsCount++;
            LastAttempt = DateTime.UtcNow;
        }
    }
}
