using Savanna.Engine.AnimalFactory;
using Savanna.Engine.Config;
using Savanna.Engine.FieldDisplayer;
using Savanna.Engine.GameMechanics;
using Savanna.Engine.UserInteraction;
using System.Threading;

namespace Savanna.Engine
{
    public class GameEngine
    {
        private SavannaFactory _factory;
        private Field _field;
        private ConsoleFieldDisplayer _displayer;

        public GameEngine()
        {
            _factory = new SavannaFactory();
            _field = new Field();
            _displayer = new ConsoleFieldDisplayer();
        }

        public void Start()
        {
            this.UserAddAnimals();
            this.EnableMovement();
        }

        private void EnableMovement()
        {
            while (_factory.Prey.Count != 0)
            {
                foreach (var hunter in _factory.Hunters)
                {
                    hunter.Hunt(_field);
                }
                _displayer.DisplayField(_field);
                Thread.Sleep(Settings.Delay);
                foreach (var hunted in _factory.Prey)
                {
                    hunted.Move(_field);
                }
                _displayer.DisplayField(_field);
                Thread.Sleep(Settings.Delay);
            }
        }

        private void UserAddAnimals()
        {
            ConsoleInputAnimalType.ShowNotification();

            bool IsEPressed = false;
            while (!IsEPressed)
            {
                var key = ConsoleInputAnimalType.GetInputChar();
                if (key == Settings.AntilopeBody)
                {
                    _factory.CreateHerbivore(_field);
                }
                else if (key == Settings.LionBody)
                {
                    _factory.CreateCarnivore(_field);
                }
                else if (key == Settings.ExitKey)
                {
                    IsEPressed = true;
                }
                _displayer.DisplayField(_field);
            }
        }
    }
}
