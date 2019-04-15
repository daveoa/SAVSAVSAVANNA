using Savanna.Engine.Config;
using Savanna.Engine.GameMechanics.Animals.AnimalTemplates;
using Savanna.Engine.GameMechanics.Models;
using Savanna.Engine.GameMechanics.Templates;
using Savanna.Engine.GameMechanics.Validators;
using System;

namespace Savanna.Engine.GameMechanics
{
    public class Movement
    {
        private Random _rand;
        private CoordinateValidator _validator;

        public Movement(Random rand, CoordinateValidator validator)
        {
            _rand = rand;
            _validator = validator;
        }

        public Coordinates Move(IField field, IFieldPresentable presentableObject, int stepsize)
        {
            var newCoords = GenerateNextMoveCoordinates(field, presentableObject, stepsize);
            field.Contents[presentableObject.CoordinateX, presentableObject.CoordinateY] = Settings.EmptyBlock;
            field.Contents[newCoords.CoordinateX, newCoords.CoordinateY] = presentableObject.Body;
            return newCoords;
        }

        private Coordinates GenerateNextMoveCoordinates(IField field, IFieldPresentable presentableObject, int stepSize)
        {
            int moveToX, moveToY;
            int minRand = -stepSize;
            int maxRand = stepSize + 1;

            do
            {
                var offsetX = _rand.Next(minRand, maxRand);
                var offsetY = _rand.Next(minRand, maxRand);
                moveToX = presentableObject.CoordinateX + offsetX;
                moveToY = presentableObject.CoordinateY + offsetY;
            } while (!(_validator.CoordinatesAreValid(moveToX, moveToY) 
                      && field.Contents[moveToX, moveToY] == Settings.EmptyBlock));

            return new Coordinates(moveToX, moveToY);
        }
    }
}
