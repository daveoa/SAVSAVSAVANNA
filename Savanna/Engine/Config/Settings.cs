namespace Savanna.Engine.Config
{
    public static class Settings
    {
        public static string AddAnimalNotification => 
            $"{AntilopeBody} - Herbivore, {LionBody} - Carnivore, press E to continue to game";

        public static char LionBody => 'L';
        public static char AntilopeBody => 'A';
        public static char EmptyBlock => '-';
        public static char ExitKey => 'E';

        public static int LionSight => 3;
        public static int AntilopeSight => 2;
        public static int Delay => 1000;
    }
}
