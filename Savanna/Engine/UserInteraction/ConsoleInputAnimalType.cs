using Savanna.Engine.Config;
using System;

namespace Savanna.Engine.UserInteraction
{
    public static class ConsoleInputAnimalType
    {
        public static char GetInputChar()
        {
            char input = Console.ReadKey().KeyChar;
            return input;
        }

        public static void ShowNotification()
        {
            Console.WriteLine(Settings.AddAnimalNotification);
        }
    }
}
