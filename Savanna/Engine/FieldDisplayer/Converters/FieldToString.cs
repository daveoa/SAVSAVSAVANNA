using Savanna.Engine.Config;
using Savanna.Engine.GameMechanics.Templates;
using System.Text;

namespace Savanna.Engine.FieldDisplayer.Converters
{
    public class FieldToString
    {
        public string Transform(IField field)
        {
            var builder = new StringBuilder();
            for (int h = 0; h < FieldDimensions.Height; h++)
            {
                for (int w = 0; w < FieldDimensions.Width; w++)
                {
                    builder.Append(field.Contents[w, h]);
                }
                builder.Append('\n');
            }
            return builder.ToString();
        }
    }
}
