using Savanna.Engine.FieldDisplayer.Converters;
using Savanna.Engine.FieldDisplayer.Templates;
using Savanna.Engine.GameMechanics.Templates;
using System;

namespace Savanna.Engine.FieldDisplayer
{
    public class ConsoleFieldDisplayer : IFieldDisplayer
    {
        private FieldToString _converter = new FieldToString();
        private string _displayStr;

        public void DisplayField(IField field)
        {
            _displayStr = _converter.Transform(field);
            Console.SetCursorPosition(0, 1);
            Console.Write(_displayStr);
        }
    }
}
