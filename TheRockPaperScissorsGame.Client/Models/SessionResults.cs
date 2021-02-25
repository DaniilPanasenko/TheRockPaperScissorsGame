using System;
namespace TheRockPaperScissorsGame.Client.Models
{
    public class SessionResults
    {
        public int WinCount { get; set; }

        public int DrawCount { get; set; }

        public int LossesCount { get; set; }

        public SessionResults()
        {
            WinCount = 0;
            DrawCount = 0;
            LossesCount = 0;
        }
    }
}
