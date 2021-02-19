using System;
namespace TheRockPaperScissorsGame.API.Services
{
    internal interface IUserBlockingService
    {
        public void NegativeLogin(string login);

        public bool TryPositiveLogin(string login);
    }
}
