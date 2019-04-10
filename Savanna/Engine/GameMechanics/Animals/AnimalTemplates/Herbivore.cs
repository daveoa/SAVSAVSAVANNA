using Savanna.Engine.GameMechanics.Templates;

namespace Savanna.Engine.GameMechanics.Animals.AnimalTemplates
{
    public abstract class Herbivore : IAnimal, IFieldPresentable
    {
        public abstract int FieldOfView { get; }
        public abstract int StepSize { get; }
        public abstract char Body { get; }
        public int CoordinateX { get; set; }
        public int CoordinateY { get; set; }

        public abstract void Move(IField field);

        public abstract void Evade(IField field);
    }
}
