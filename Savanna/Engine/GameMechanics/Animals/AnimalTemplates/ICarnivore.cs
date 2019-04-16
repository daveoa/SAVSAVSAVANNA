using Savanna.Engine.GameMechanics.Models;
using Savanna.Engine.GameMechanics.Templates;

namespace Savanna.Engine.GameMechanics.Animals.AnimalTemplates
{
    public interface ICarnivore: IAnimal, IFieldPresentable
    {
        void Hunt(IField field);

        void Eat(IField field, Coordinates preyLocation);
    }
}
