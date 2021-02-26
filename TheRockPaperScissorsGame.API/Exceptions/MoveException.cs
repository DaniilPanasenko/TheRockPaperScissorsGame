using System;

namespace TheRockPaperScissorsGame.API.Exceptions
{
    public class MoveException:Exception
    {
        public MoveException(string message) : base(message)
        {
        }
    }
}
