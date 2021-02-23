using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TheRockPaperScissorsGame.API.Exceptions
{
    public class MoveException:Exception
    {
        public MoveException(string message) : base(message)
        {
        }
    }
}
