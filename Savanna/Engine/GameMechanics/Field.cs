using Savanna.Engine.Config;
using Savanna.Engine.GameMechanics.Templates;

namespace Savanna.Engine.GameMechanics
{
    public class Field : IField
    {
        public char[,] Contents { get; set; }

        public Field()
        {
            Contents = new char[FieldDimensions.Width, FieldDimensions.Height];
            for (int h = 0; h < FieldDimensions.Height; h++)
            {
                for (int w = 0; w < FieldDimensions.Width; w++)
                {
                    Contents[w, h] = Settings.EmptyBlock;
                }
            }
        }
    }
}
