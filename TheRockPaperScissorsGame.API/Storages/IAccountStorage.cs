namespace TheRockPaperScissorsGame.API.Storages
{
    interface IAccountStorage
    {
        Account FindAccount(string login, string password);
        Account FindAccount(string login);
        bool AddAccount(Account account);
    }
}