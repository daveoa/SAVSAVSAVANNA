using Savanna.Engine.AnimalFactory;
using Savanna.Engine.Config;
using Savanna.Engine.FieldDisplayer;
using Savanna.Engine.GameMechanics;
using Savanna.Engine.UserInteraction;
using System;
using System.Threading;

namespace Savanna.Engine
{
    public class GameEngine
    {
        private SavannaFactory _factory;
        private Field _field;
        private ConsoleFieldDisplayer _displayer;
        private bool _isGameOver = false;

        public GameEngine()
        {
            _factory = new SavannaFactory();
            _field = new Field();
            _displayer = new ConsoleFieldDisplayer();
        }

        public void Start()
        {
            do
            {
                this.UserAddAnimals();
                this.EnableMovement();
            } while (!_isGameOver);
        }

        private void EnableMovement()
        {
            foreach (var hunter in _factory.Hunters)
            {
                hunter.Hunt(_field);
            }
            _displayer.DisplayField(_field);
            Thread.Sleep(Settings.Delay);
            _factory.Prey.RemoveAll(item => _field.Contents[item.CoordinateX, item.CoordinateY] != item.Body);
            foreach (var hunted in _factory.Prey)
            {
                hunted.Evade(_field);
            }
            _displayer.DisplayField(_field);
            Thread.Sleep(Settings.Delay);
        }

        private void UserAddAnimals()
        {
            ConsoleInputAnimalType.ShowNotification();
            while (Console.KeyAvailable)
            {
                if (Console.ReadKey(true).KeyChar == Char.ToLower(Settings.AntilopeBody))
                {
                    _factory.CreateHerbivore(_field);
                }
                if (Console.ReadKey(true).KeyChar == Char.ToLower(Settings.LionBody))
                {
                    _factory.CreateCarnivore(_field);
                }
            }
            _displayer.DisplayField(_field);
        }
    }
}
