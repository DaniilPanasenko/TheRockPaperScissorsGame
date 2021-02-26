namespace TheRockPaperScissorsGame.API.Storages
{
    public interface ITokenStorage
    {
        public string GenerateToken(string login);

        public string GetLogin(string token);
    }
}
