using System;
using Microsoft.Extensions.Logging;

namespace YandexDiskFileUploader.Logger
{
    public static class ConsoleLogger
    {
        private static object _lock = new object();

        private static int _id;
        public static int GetId()
        {
            lock (_lock)
            {
                return _id++;
            }
        }

        public static void Write(int id, string value)
        {
            lock (_lock)
            {
                Console.SetCursorPosition(0, id);
                Console.Write(value);
            }
        }

        public static void Error(int id, string value)
        {
            lock (_lock)
            {
                Console.SetCursorPosition(0, id);
                ConsoleColor color = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write(value);
                Console.ForegroundColor = color;
            }
        }
    }
}
