using System;


namespace AutoBattle
{
    public static class Messages
    {

        public static void ColoredWriteLine(string message, ConsoleColor foregroungColor, ConsoleColor backgroundColor)
        {
            Console.BackgroundColor = backgroundColor;
            ColoredWriteLine(message, foregroungColor);            
        }
        public static void ColoredWriteLine(string message, ConsoleColor foregroungColor)
        {            
            Console.ForegroundColor = foregroungColor;
            Console.WriteLine(message);
            Console.ResetColor();
        }
        public static string ColoredWrite(string message, ConsoleColor foregroungColor)
        {
            Console.ForegroundColor = foregroungColor;
            Console.Write(message);
            Console.ResetColor();
            return message;
        }


    }
}
