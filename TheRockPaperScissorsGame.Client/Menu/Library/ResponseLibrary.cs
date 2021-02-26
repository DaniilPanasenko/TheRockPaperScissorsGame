using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace TheRockPaperScissorsGame.Client.Menu.Library
{
    public static class ResponseLibrary
    {
        public static async Task RepeatOperationWithMessageAsync<T>(HttpResponseMessage response)
        {
            var jsonResponse = await response.Content.ReadAsStringAsync();
            var textResponse = JsonSerializer.Deserialize<T>(jsonResponse).ToString();
            if (typeof(T).Equals(typeof(string)))
            {
<<<<<<< HEAD
                textResponse = ParseEnumStringToSring(textResponse);
=======
                textResponse = ParseEnumStringToString(textResponse);
>>>>>>> 8f15b79f4f62d77b3948675ba70f3eabb0843599
            }
            MenuLibrary.WriteLineColor($"\n{textResponse}", ConsoleColor.Red);
            MenuLibrary.WriteLineColor("Please repeat operation.", ConsoleColor.White);
            Thread.Sleep(1000);
        }

        public static void UnknownResponse()
        {
            MenuLibrary.WriteLineColor("\nSorry, something went wrong\n", ConsoleColor.Red);
            Thread.Sleep(1000);
        }

        public static void SuccessfullyOperation(string operation)
        {
            MenuLibrary.WriteLineColor($"\nSuccessfully {operation}\n", ConsoleColor.Green);
            Thread.Sleep(1000);
        }

        public static async Task GameFinishedResponseAsync(HttpResponseMessage response)
        {
            var exception = await response.Content.ReadAsStringAsync();
            exception = JsonSerializer.Deserialize<string>(exception);
<<<<<<< HEAD
            exception = ParseEnumStringToSring(exception);
=======
            exception = ParseEnumStringToString(exception);
>>>>>>> 8f15b79f4f62d77b3948675ba70f3eabb0843599
            MenuLibrary.WriteLineColor($"\nSorry, your game is finished because of {exception}\n", ConsoleColor.Red);
            Thread.Sleep(3000);
        }

<<<<<<< HEAD
        public static string ParseEnumStringToSring(string enumSring)
        {
            string result = "";
            bool isFirst = true;
            foreach(var ch in enumSring)
            {
                if(char.IsUpper(ch) && !isFirst)
=======
        public static string ParseEnumStringToString(string enumString)
        {
            string result = "";
            bool isFirst = true;
            foreach (var ch in enumString)
            {
                if (char.IsUpper(ch) && !isFirst)
>>>>>>> 8f15b79f4f62d77b3948675ba70f3eabb0843599
                {
                    result += " " + char.ToLower(ch);
                }
                else
                {
                    result += ch;
                    isFirst = false;
                }
            }
            return result;
        }
    }
}