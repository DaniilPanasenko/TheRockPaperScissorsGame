using System;

namespace TheRockPaperScissorsGame.Client.Menu.Library
{
    public static class MenuLibrary
    {
        public static int InputMenuItemNumber(string menuName, string[] commands)
        {
            WriteLineColor($"\n{menuName}:", ConsoleColor.Yellow);
            for (int i = 0; i < commands.Length; i++)
            {
                Console.WriteLine($"{i + 1} - {commands[i]}");
            }
            WriteLineColor("\nEnter the command number...", ConsoleColor.DarkCyan);
            int command;
            while (!int.TryParse(Console.ReadLine(), out command) || command < 1 || command > commands.Length)
            {
                WriteLineColor("You entered incorrect command", ConsoleColor.Red);
                WriteLineColor("\nEnter the command number...", ConsoleColor.DarkCyan);
            }
            return command;
        }

        public static string InputStringValue(string valueName)
        {
            WriteLineColor($"Enter {valueName} ...", ConsoleColor.DarkCyan);
            string value;
            value = Console.ReadLine();
            while (string.IsNullOrEmpty(value.Trim()))
            {
                WriteLineColor($"You entered empty {valueName}", ConsoleColor.Red);
                WriteLineColor($"\nEnter {valueName} ...", ConsoleColor.DarkCyan);
                value = Console.ReadLine();
            }
            return value;
        }

        public static int InputIntegerValue(string valueName, int min, int max)
        {
            WriteLineColor($"Enter {valueName} ...", ConsoleColor.DarkCyan);
            int value;
            while (!int.TryParse(Console.ReadLine(), out value) || value < min || value > max)
            {
                WriteLineColor($"You entered incorrect {valueName}", ConsoleColor.Red);
                WriteLineColor($"\nEnter {valueName} ...", ConsoleColor.DarkCyan);
            }
            return value;
        }

        public static void WriteLineColor(string text, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(text);
            Console.ForegroundColor = ConsoleColor.White;
        }

        public static void WriteColor(string text, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.Write(text);
            Console.ForegroundColor = ConsoleColor.White;
        }

        public static void PressAnyKey()
        {
            WriteLineColor("\nPress any key to continue", ConsoleColor.DarkCyan);
            Console.ReadKey();
        }

        public static void Clear()
        {
            Console.Clear();
        }
    }
}
