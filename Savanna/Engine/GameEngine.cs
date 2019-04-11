using Savanna.Engine.AnimalFactory;
using Savanna.Engine.Config;
using Savanna.Engine.FieldDisplayer;
using Savanna.Engine.GameMechanics;
using Savanna.Engine.GameMechanics.Templates;
using Savanna.Engine.UserInteraction;
using System.Threading;

namespace Savanna.Engine
{
    public class GameEngine
    {
        private SavannaFactory _factory;
        private IField _field;
        private ConsoleFieldDisplayer _displayer;
        private ConsoleUserAddAnimals _user;
        private bool _isGameOver = false;

        public GameEngine()
        {
            _factory = new SavannaFactory();
            _field = new Field();
            _displayer = new ConsoleFieldDisplayer();
            _user = new ConsoleUserAddAnimals(_factory, _field);
        }

        public void Start()
        {
            do
            {
                UserAddAnimals();
                EnableMovement();
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
            _user.AddAnimals();
            _displayer.DisplayField(_field);
        }
    }
}
