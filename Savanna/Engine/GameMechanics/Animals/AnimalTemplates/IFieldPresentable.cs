namespace Savanna.Engine.GameMechanics.Animals.AnimalTemplates
{
    public interface IFieldPresentable
    {
        char Body { get; }
        int CoordinateX { get; set; }
        int CoordinateY { get; set; }
    }
}
