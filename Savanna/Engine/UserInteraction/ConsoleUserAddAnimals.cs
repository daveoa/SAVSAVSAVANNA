using Savanna.Engine.AnimalFactory.Templates;
using Savanna.Engine.Config;
using Savanna.Engine.GameMechanics.Templates;
using System;

namespace Savanna.Engine.UserInteraction
{
    public class ConsoleUserAddAnimals
    {
        private ISavannaFactory _factory;
        private IField _field;

        public ConsoleUserAddAnimals(ISavannaFactory factory, IField field)
        {
            _factory = factory;
            _field = field;
        }

        public void AddAnimals()
        {
            Console.WriteLine(Settings.AddAnimalNotification);
            char key;
            while (Console.KeyAvailable)
            {
                if ((key = Console.ReadKey(true).KeyChar) == Char.ToLower(Settings.AntilopeBody)
                    || (key = Console.ReadKey(true).KeyChar) == Char.ToLower(Settings.LionBody))
                {
                    _factory.CreateAnimal(_field, Char.ToUpper(key));
                }
            }
        }
    }
}
