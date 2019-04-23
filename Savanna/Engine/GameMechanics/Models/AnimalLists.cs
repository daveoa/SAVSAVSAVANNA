using Savanna.Engine.GameMechanics.Animals.AnimalTemplates;
using System.Collections.Generic;

namespace Savanna.Engine.GameMechanics.Models
{
    public class AnimalLists
    {
        public List<IHerbivore> Prey;
        public List<ICarnivore> Hunters;

        public AnimalLists()
        {
            Prey = new List<IHerbivore>();
            Hunters = new List<ICarnivore>();
        }
    }
}
