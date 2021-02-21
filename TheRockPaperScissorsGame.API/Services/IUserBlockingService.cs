using System;
namespace TheRockPaperScissorsGame.API.Services
{
    internal interface IUserBlockingService
    {
        public void NegativeLogin(string login);

        public bool IsBlocked(string login);

        public void PositiveLogin(string login);
    }
}
