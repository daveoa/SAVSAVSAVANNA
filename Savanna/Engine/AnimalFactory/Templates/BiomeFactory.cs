using Savanna.Engine.GameMechanics.Templates;

namespace Savanna.Engine.AnimalFactory.Templates
{
    public interface IBiomeFactory
    {
        void CreateAnimal(IField field, char animalBody);
    }
}
