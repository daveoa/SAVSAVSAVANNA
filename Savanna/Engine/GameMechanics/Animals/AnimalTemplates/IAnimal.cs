using Savanna.Engine.GameMechanics.Templates;

namespace Savanna.Engine.GameMechanics.Animals.AnimalTemplates
{
    public interface IAnimal
    {
        int FieldOfView { get; }
        int StepSize { get; }

        void Move(IField field);
    }
}
