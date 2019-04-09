using Savanna.Engine.GameMechanics.Templates;

namespace Savanna.Engine.AnimalFactory.Templates
{
    public interface IBiomeFactory
    {
        void CreateHerbivore(IField field);

        void CreateCarnivore(IField field);
    }
}
