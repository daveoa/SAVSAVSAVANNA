namespace Savanna.Engine.Config
{
    public static class Settings
    {
        public static string AddAnimalNotification => 
            $"{AntilopeBody} - Herbivore, {LionBody} - Carnivore";

        public static char LionBody => 'L';
        public static char AntilopeBody => 'A';
        public static char EmptyBlock => '-';

        public static int LionSight => 6;
        public static int AntilopeSight => 5;
        public static int LionStep => 2;
        public static int AntilopeStep => 3;

        public const int Delay = 500;
    }
}
