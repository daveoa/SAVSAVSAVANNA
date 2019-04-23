using Savanna.Engine.AnimalFactory;
using Savanna.Engine.AnimalFactory.Templates;
using Savanna.Engine.Config;
using Savanna.Engine.FieldDisplayer;
using Savanna.Engine.FieldDisplayer.Converters;
using Savanna.Engine.FieldDisplayer.Templates;
using Savanna.Engine.GameMechanics;
using Savanna.Engine.GameMechanics.Models;
using Savanna.Engine.GameMechanics.Templates;
using Savanna.Engine.GameMechanics.Validators;
using Savanna.Engine.LoadAssembly;
using Savanna.Engine.UserInteraction;
using System;
using System.Collections.Generic;
using System.Threading;

namespace Savanna.Engine
{
    public class GameEngine
    {
        private ISavannaFactory _factory;
        private IField _field;
        private IFieldDisplayer _displayer;
        private ConsoleUserAddAnimals _user;
        private AnimalLists _animalLists;
        private AssemblyLoader _assemblyLoader;
        private EnemyMatchmaking _matchmaker;

        public GameEngine()
        {
            var rand = new Random();
            var spawn = new Spawner(rand);
            var validator = new CoordinateValidator();
            var pointCalc = new AxisPointCalculations();
            var correctPlacement = new PlacementCorrection();
            var predatorSpecial = new PredatorEssentials(validator, pointCalc, correctPlacement);
            var preySpecial = new PreyEssentials(validator, correctPlacement);
            var standardMovement = new Movement(rand, validator);
            var fieldToStrConverter = new FieldToString();
            _factory = new SavannaFactory
                (validator, spawn, standardMovement, predatorSpecial, preySpecial);
            _field = new Field();
            _displayer = new ConsoleFieldDisplayer(fieldToStrConverter);
            _user = new ConsoleUserAddAnimals
                (standardMovement, validator, predatorSpecial, preySpecial, spawn);
            _animalLists = new AnimalLists();
            _assemblyLoader = new AssemblyLoader(validator, standardMovement, preySpecial, predatorSpecial);
        }

        public void Start()
        {
            var _isGameOver = false;
            var animalBodyAndType = _assemblyLoader.RetrieveAnimalBodies();
            _matchmaker = new EnemyMatchmaking(animalBodyAndType);

            do
            {
                UserAddAnimals(animalBodyAndType);
                EnableMovement();
            } while (!_isGameOver);
        }

        private void EnableMovement()
        {
            foreach (var hunter in _animalLists.Hunters)
            {
                hunter.Hunt(_field, _matchmaker.GetEnemy(hunter.Body));
            }
            _displayer.DisplayField(_field);
            Thread.Sleep(Settings.Delay);
            _animalLists.Prey.RemoveAll(item => _field.Contents[item.CoordinateX, item.CoordinateY] != item.Body);
            foreach (var hunted in _animalLists.Prey)
            {
                hunted.Evade(_field, _matchmaker.GetEnemy(hunted.Body));
            }
            _displayer.DisplayField(_field);
            Thread.Sleep(Settings.Delay);
        }

        private void UserAddAnimals(Dictionary<char, Type> bodyAndType)
        {
            _displayer.DisplayField(_field);
            _animalLists = _user.AddAnimals(_animalLists, _field, bodyAndType);
        }
    }
}
