namespace Savanna.Engine.Config
{
    public static class Settings
    {
        public static string AddAnimalNotification => 
            $"{AntilopeBody} - Herbivore, {LionBody} - Carnivore";

        public static char LionBody => 'L';
        public static char AntilopeBody => 'A';
        public static char EmptyBlock => '-';

        public static int LionSight => 3;
        public static int AntilopeSight => 2;
        public static int Delay => 500;
    }
}
