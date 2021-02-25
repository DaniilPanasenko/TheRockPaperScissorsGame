using System.Collections.Generic;

namespace TheRockPaperScissorsGame.Client.Models
{
    public class UserValidaionResponse
    {
        public List<string> Login { get; set; }

        public List<string> Password { get; set; }

        public override string ToString()
        {
            string result = "";
            if (Login != null)
            {
                foreach(var exception in Login)
                {
                    result += exception + "\n";
                }
            }
            if (Password != null)
            {
                foreach (var exception in Password)
                {
                    result += exception + "\n";
                }
            }
            return result;
        }
    }
}
