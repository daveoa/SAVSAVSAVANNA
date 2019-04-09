using System;

namespace Savanna.Engine.UserInteraction.KeyTriggers
{
    public static class EscapeKeyPress
    {
        public static bool EscIsNotPressed()
        {
            return !Console.KeyAvailable || Console.ReadKey(true).Key != ConsoleKey.Escape;
        }
    }
}
