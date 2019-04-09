using Savanna.Engine.GameMechanics.Animals.AnimalTemplates;
using Savanna.Engine.GameMechanics.Models;
using Savanna.Engine.GameMechanics.Templates;
using System;

namespace Savanna.Engine.GameMechanics
{
    public class Spawner : ISpawner
    {
        Random _rand = new Random();

        public IField SetSpawnPoint(IField field, IFieldPresentable fieldProperty)
        {
            var coord = this.GenerateSpawnPoint(fieldProperty);
            field.Contents[coord.CoordinateX, coord.CoordinateY] = fieldProperty.Body;
            return field;
        }

        private Coordinates GenerateSpawnPoint(IFieldPresentable fieldProperty)
        {
            fieldProperty.CoordinateX = _rand.Next(0, Config.FieldDimensions.Width);
            fieldProperty.CoordinateY = _rand.Next(0, Config.FieldDimensions.Height);
            return new Coordinates(fieldProperty.CoordinateX, fieldProperty.CoordinateY);
        }
    }
}
