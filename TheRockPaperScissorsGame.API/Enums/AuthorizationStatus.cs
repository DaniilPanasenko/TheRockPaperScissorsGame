namespace TheRockPaperScissorsGame.API.Enums
{
    public enum AuthorizationStatus
    {
        OK,
        IncorrectLogin,
        IncorrectPassword,
        BlockedAccountFor1Minute,
        LoginAlreadyExist,
        Undefined
    }
}
