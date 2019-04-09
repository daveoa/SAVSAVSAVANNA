using Savanna.Engine.GameMechanics.Animals.AnimalTemplates;

namespace Savanna.Engine.GameMechanics.Templates
{
    public interface ISpawner
    {
        IField SetSpawnPoint(IField field, IFieldPresentable fieldProperty);
    }
}
