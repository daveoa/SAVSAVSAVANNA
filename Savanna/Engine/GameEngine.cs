using Savanna.Engine.AnimalFactory;
using Savanna.Engine.AnimalFactory.Templates;
using Savanna.Engine.Config;
using Savanna.Engine.FieldDisplayer;
using Savanna.Engine.FieldDisplayer.Converters;
using Savanna.Engine.FieldDisplayer.Templates;
using Savanna.Engine.GameMechanics;
using Savanna.Engine.GameMechanics.Templates;
using Savanna.Engine.GameMechanics.Validators;
using Savanna.Engine.UserInteraction;
using System;
using System.Threading;

namespace Savanna.Engine
{
    public class GameEngine
    {
        private ISavannaFactory _factory;
        private IField _field;
        private IFieldToString _fieldToStrConverter;
        private ConsoleFieldDisplayer _displayer;
        private ConsoleUserAddAnimals _user;
        private Random _rand;
        private ISpawner _spawn;
        private Movement _standardMovement;
        private CoordinateValidator _validator;
        private AxisPointCalculations _pointCalc;
        private PredatorEssentials _predSpecial;
        private PreyEssentials _preySpecial;
        private PlacementCorrection _correct;
        private bool _isGameOver = false;

        public GameEngine()
        {
            _rand = new Random();
            _spawn = new Spawner(_rand);
            _validator = new CoordinateValidator();
            _pointCalc = new AxisPointCalculations();
            _correct = new PlacementCorrection();
            _predSpecial = new PredatorEssentials(_validator, _pointCalc, _correct);
            _preySpecial = new PreyEssentials(_validator, _correct);
            _standardMovement = new Movement(_rand, _validator);
            _factory = new SavannaFactory
                (_validator, _spawn, _standardMovement, _pointCalc, _predSpecial, _preySpecial);
            _field = new Field();
            _fieldToStrConverter = new FieldToString();
            _displayer = new ConsoleFieldDisplayer(_fieldToStrConverter);
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
