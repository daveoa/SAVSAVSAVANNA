using Savanna.Engine.AnimalFactory.Templates;
using Savanna.Engine.Config;
using Savanna.Engine.GameMechanics;
using Savanna.Engine.GameMechanics.Animals;
using Savanna.Engine.GameMechanics.Models;
using Savanna.Engine.GameMechanics.Templates;
using Savanna.Engine.GameMechanics.Validators;

namespace Savanna.Engine.AnimalFactory
{
    public class SavannaFactory : ISavannaFactory
    {
        private PredatorEssentials _predSpecial;
        private PreyEssentials _preySpecial;
        private ISpawner _spawn;
        private Movement _standardMovement;
        private CoordinateValidator _validator;

        public SavannaFactory
            (CoordinateValidator validator, ISpawner spawn, Movement movement,
            PredatorEssentials predSpecial, PreyEssentials preySpecial)
        {
            _predSpecial = predSpecial;
            _preySpecial = preySpecial;
            _validator = validator;
            _spawn = spawn;
            _standardMovement = movement;
        }

        public void CreateAnimal(IField field, AnimalLists animalLists, char animalBody)
        {
            if (animalBody == Settings.AntilopeBody)
            {
                var creature = new Antilope(_standardMovement, _validator, _preySpecial);
                _spawn.SetSpawnPoint(field, creature);
                animalLists.Prey.Add(creature);
            }
            else
            {
                var creature = new Lion(_standardMovement, _validator, _predSpecial);
                _spawn.SetSpawnPoint(field, creature);
                animalLists.Hunters.Add(creature);
            }
        }
    }
}
