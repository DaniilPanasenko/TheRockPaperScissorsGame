using System;
using TheRockPaperScissorsGame.API.Enums;

namespace TheRockPaperScissorsGame.API.Exceptions
{
    public class GameFinishedException : Exception
    {
        public GameEndReason Status { get; set; }

        public GameFinishedException(GameEndReason status) : base()
        {
            Status = status;
        }
    }
}
