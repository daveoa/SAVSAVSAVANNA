using Savanna.Engine.GameMechanics.Templates;

namespace Savanna.Engine.GameMechanics.Animals.AnimalTemplates
{
    public interface IHerbivore : IAnimal, IFieldPresentable
    {
        void Evade(IField field, char predatorBody);
    }
}
