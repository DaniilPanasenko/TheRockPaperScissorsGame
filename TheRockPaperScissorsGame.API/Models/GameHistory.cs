using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TheRockPaperScissorsGame.API.Models
{
    public class GameHistory
    {
        public GameHistory()
        {
        }

        // maybe we can think of much better name for this variable))
        public string User { get; set; }
        public int WinsAmount { get; set; }
        public int LossAmount { get; set; }

        // need to think about: actually user is persistent, 
        // WinsAmount and LossAmount are persistent too, they just will be incremented
        // but be decided that we also need to store the moves (paper, scissors or rock)
        // and this should probaly be a collection, in which we will store eahc user's move 
        // and we can use it for moves' statictics. What do you think?
    }
}
