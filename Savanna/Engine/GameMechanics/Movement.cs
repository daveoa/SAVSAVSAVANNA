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

        public Coordinates Move(IField field, IFieldPresentable presentableObj, int stepsize)
        {
            var newCoords = GenerateNextMoveCoordinates(field, presentableObj, stepsize);
            field.Contents[presentableObj.CoordinateX, presentableObj.CoordinateY] = Settings.EmptyBlock;
            field.Contents[newCoords.CoordinateX, newCoords.CoordinateY] = presentableObj.Body;
            return newCoords;
        }

        private Coordinates GenerateNextMoveCoordinates(IField field, IFieldPresentable presentableObj, int stepSize)
        {
            int moveToX;
            int moveToY;
            int minRand = -stepSize;
            int maxRand = stepSize + 1;

            do
            {
                do
                {
                    var offsetX = _rand.Next(minRand, maxRand);
                    var offsetY = _rand.Next(minRand, maxRand);
                    moveToX = presentableObj.CoordinateX + offsetX;
                    moveToY = presentableObj.CoordinateY + offsetY;
                } while (!_validator.CoordinatesAreValid(moveToX, moveToY));
            } while (field.Contents[moveToX, moveToY] != Settings.EmptyBlock);
            return new Coordinates(moveToX, moveToY);
        }
    }
}
