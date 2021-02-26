using TheRockPaperScissorsGame.API.Enums;

namespace TheRockPaperScissorsGame.API.Models
{
    public class GameOptions
    {
        public RoomType RoomType { get; set; }

        public string RoomNumber { get; set; }
    }
}
