using Savanna.Engine.GameMechanics.Animals.AnimalTemplates;
using Savanna.Engine.GameMechanics.Templates;
using System.Collections.Generic;

namespace Savanna.Engine.AnimalFactory.Templates
{
    public interface ISavannaFactory
    {
        List<IHerbivore> Prey { get; set; }
        List<ICarnivore> Hunters { get; set; }
        void CreateAnimal(IField field, char animalBody);
    }
}
