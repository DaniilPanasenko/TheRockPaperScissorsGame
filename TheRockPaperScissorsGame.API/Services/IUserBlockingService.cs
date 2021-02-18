using System;
namespace TheRockPaperScissorsGame.API.Services
{
    public interface IUserBlockingService
    {
        public void NegativeLogin(string login);

        public bool TryPositiveLogin(string login);
    }
}
