using Savanna.Engine.AnimalFactory.Templates;
using Savanna.Engine.Config;
using Savanna.Engine.GameMechanics;
using Savanna.Engine.GameMechanics.Animals;
using Savanna.Engine.GameMechanics.Animals.AnimalTemplates;
using Savanna.Engine.GameMechanics.Templates;
using Savanna.Engine.GameMechanics.Validators;
using System;
using System.Collections.Generic;

namespace Savanna.Engine.AnimalFactory
{
    public class SavannaFactory : IBiomeFactory
    {
        public List<Herbivore> Prey;
        public List<Carnivore> Hunters;
        private ISpawner _spawn;
        private Movement _standardMovement;
        private CoordinateValidator _validator;
        private Random _rand;

        public SavannaFactory()
        {
            Prey = new List<Herbivore>();
            Hunters = new List<Carnivore>();
            _validator = new CoordinateValidator();
            _rand = new Random();
            _spawn = new Spawner(_rand);
            _standardMovement = new Movement(_rand, _validator);
        }

        public void CreateAnimal(IField field, char animalBody)
        {
            if (animalBody == Settings.AntilopeBody)
            {
                var creature = new Antilope(_standardMovement, _validator);
                _spawn.SetSpawnPoint(field, creature);
                Prey.Add(creature);
            }
            else if (animalBody == Settings.LionBody)
            {
                var creature = new Lion(_standardMovement, _validator);
                _spawn.SetSpawnPoint(field, creature);
                Hunters.Add(creature);
            }
        }
    }
}
