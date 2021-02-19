using System.Threading.Tasks;
using TheRockPaperScissorsGame.API.Models;

namespace TheRockPaperScissorsGame.API.Storages
{
    internal interface IAccountStorage
    {
        Task<Account> FindAccountAsync(string login);

        Task<bool> AddAccountAsync(Account account);
    }
}