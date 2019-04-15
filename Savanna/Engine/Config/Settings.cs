namespace Savanna.Engine.Config
{
    public static class Settings
    {
        public static string AddAnimalNotification = 
            $"{AntilopeBody} - Herbivore, {LionBody} - Carnivore";

        public const char LionBody = 'L';
        public const char AntilopeBody = 'A';
        public const char EmptyBlock = '\u00b7';

        public const int LionSight = 6;
        public const int AntilopeSight = 5;
        public const int LionStep = 2;
        public const int AntilopeStep = 3;

        public const int Delay = 500;
    }
}
