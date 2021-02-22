using System;
namespace TheRockPaperScissorsGame.API.Exceptions
{
    public class RoomConnectionException : Exception
    {
        public RoomConnectionException(string message) : base(message)
        {
        }
    }
}
