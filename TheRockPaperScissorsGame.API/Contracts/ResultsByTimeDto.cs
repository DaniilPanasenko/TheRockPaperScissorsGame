using System;
namespace TheRockPaperScissorsGame.API.Contracts
{
    public class ResultsByTimeDto
    {
        public ResultsByTimeDto(string time)
        {
            Time = time;
            WinCount = 0;
            LossCount = 0;
        }

        public string Time { get; set; }

        public int WinCount { get; set; }

        public int LossCount { get; set; }
    }
}
