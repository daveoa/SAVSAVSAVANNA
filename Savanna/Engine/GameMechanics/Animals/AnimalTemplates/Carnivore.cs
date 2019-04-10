using Savanna.Engine.GameMechanics.Models;
using Savanna.Engine.GameMechanics.Templates;

namespace Savanna.Engine.GameMechanics.Animals.AnimalTemplates
{
    public abstract class Carnivore: IAnimal, IFieldPresentable
    {
        public abstract int FieldOfView { get; }
        public abstract int StepSize { get; }
        public abstract char Body { get; }
        public int CoordinateX { get; set; }
        public int CoordinateY { get; set; }

        public abstract void Move(IField field);

        public abstract void Hunt(IField field);

        public abstract void Eat(IField field, Coordinates preyLocation);
    }
}
