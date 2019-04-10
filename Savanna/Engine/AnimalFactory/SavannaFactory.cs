using Savanna.Engine.AnimalFactory.Templates;
using Savanna.Engine.GameMechanics;
using Savanna.Engine.GameMechanics.Animals;
using Savanna.Engine.GameMechanics.Animals.AnimalTemplates;
using Savanna.Engine.GameMechanics.Templates;
using System.Collections.Generic;

namespace Savanna.Engine.AnimalFactory
{
    public class SavannaFactory : IBiomeFactory
    {
        public List<Herbivore> Prey;
        public List<Carnivore> Hunters;
        private Spawner _spawn = new Spawner();

        public SavannaFactory()
        {
            Prey = new List<Herbivore>();
            Hunters = new List<Carnivore>();
            _spawn = new Spawner();
        }

        public void CreateCarnivore(IField field)
        {
            var creature = new Lion();
            _spawn.SetSpawnPoint(field, creature);
            Hunters.Add(creature);
        }

        public void CreateHerbivore(IField field)
        {
            var creature = new Antilope();
            _spawn.SetSpawnPoint(field, creature);
            Prey.Add(creature);
        }
    }
}
