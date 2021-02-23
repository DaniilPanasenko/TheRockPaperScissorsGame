namespace TheRockPaperScissorsGame.API.Contracts
{
    public class UserResultDto<T>
    {
        public string Login { get; set; }

        public T Result { get; set; }
    }
}
