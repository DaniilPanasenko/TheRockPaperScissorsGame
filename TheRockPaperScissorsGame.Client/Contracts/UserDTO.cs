namespace TheRockPaperScissorsGame.Client.Contracts
{
    public class UserDto
    {
        public UserDto(string login, string password)
        {
            Login = login;
            Password = password;
        }

        public string Login { get; set; }

        public string Password { get; set; }
    }
}
