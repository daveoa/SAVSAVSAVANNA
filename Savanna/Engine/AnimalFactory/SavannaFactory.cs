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
    public class SavannaFactory : ISavannaFactory
    {
        public List<IHerbivore> Prey { get; set; }
        public List<ICarnivore> Hunters { get; set; }
        private ISpawner _spawn;
        private Movement _standardMovement;
        private CoordinateValidator _validator;

        public SavannaFactory(CoordinateValidator validator, ISpawner spawn, Movement movement)
        {
            Prey = new List<IHerbivore>();
            Hunters = new List<ICarnivore>();
            _validator = validator;
            _spawn = spawn;
            _standardMovement = movement;
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
