using Savanna.Engine.GameMechanics.Models;
using Savanna.Engine.GameMechanics.Templates;

namespace Savanna.Engine.AnimalFactory.Templates
{
    public interface ISavannaFactory
    {
        void CreateAnimal(IField field, AnimalLists lists, char animalBody);
    }
}
