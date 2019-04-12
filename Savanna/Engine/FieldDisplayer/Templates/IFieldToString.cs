using Savanna.Engine.GameMechanics.Templates;

namespace Savanna.Engine.FieldDisplayer.Templates
{
    public interface IFieldToString
    {
        string Transform(IField field);
    }
}
