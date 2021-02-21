using System;
using TheRockPaperScissorsGame.API.Enums;

namespace TheRockPaperScissorsGame.API.Exceptions
{
    public class AuthorizationException : Exception
    {
        public AuthorizationStatus Status { get; set; }

        public AuthorizationException(AuthorizationStatus status) : base()
        {
            Status = status;
        }
    }
}
