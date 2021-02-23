using System;
namespace TheRockPaperScissorsGame.Client.Contracts
{
    public class UserDTO
    {
        public UserDTO(string login, string password)
        {
            Login = login;
            Password = password;
        }

        public string Login { get; set; }

        public string Password { get; set; }
    }
}
