using TheRockPaperScissorsGame.API.Models;

namespace TheRockPaperScissorsGame.API.Storages
{
    interface IAccountStorage
    {
        Account FindAccount(string login, string password);
        bool AddAccount(Account account);
    }
}