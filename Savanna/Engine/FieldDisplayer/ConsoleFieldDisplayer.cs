using Savanna.Engine.FieldDisplayer.Templates;
using Savanna.Engine.GameMechanics.Templates;
using System;

namespace Savanna.Engine.FieldDisplayer
{
    public class ConsoleFieldDisplayer : IFieldDisplayer
    {
        private IFieldToString _converter;
        private string _displayStr;

        public ConsoleFieldDisplayer(IFieldToString fieldToStringConverter)
        {
            _converter = fieldToStringConverter;
        }

        public void DisplayField(IField field)
        {
            _displayStr = _converter.Transform(field);
            Console.SetCursorPosition(0, 0);
            Console.Write(_displayStr);
        }
    }
}
