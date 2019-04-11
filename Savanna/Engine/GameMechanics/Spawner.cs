using Savanna.Engine.Config;
using Savanna.Engine.GameMechanics.Animals.AnimalTemplates;
using Savanna.Engine.GameMechanics.Models;
using Savanna.Engine.GameMechanics.Templates;
using System;

namespace Savanna.Engine.GameMechanics
{
    public class Spawner : ISpawner
    {
        private Random _rand;

        public Spawner(Random random)
        {
            _rand = random;
        }

        public IField SetSpawnPoint(IField field, IFieldPresentable fieldProperty)
        {
            var coord = GenerateSpawnPoint(field, fieldProperty);
            field.Contents[coord.CoordinateX, coord.CoordinateY] = fieldProperty.Body;
            return field;
        }

        private Coordinates GenerateSpawnPoint(IField field, IFieldPresentable fieldProperty)
        {
            int xPos;
            int yPos;
            do
            {
                xPos = _rand.Next(0, FieldDimensions.Width);
                yPos = _rand.Next(0, FieldDimensions.Height);
            } while (!IsSpawnTaken(field, xPos, yPos));
            fieldProperty.CoordinateX = xPos;
            fieldProperty.CoordinateY = yPos;
            return new Coordinates(fieldProperty.CoordinateX, fieldProperty.CoordinateY);
        }

        private bool IsSpawnTaken(IField field, int xPos, int yPos)
        {
            return field.Contents[xPos, yPos] == Settings.EmptyBlock;
        }
    }
}
