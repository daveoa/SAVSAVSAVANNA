using Savanna.Engine.Config;
using Savanna.Engine.GameMechanics.Animals.AnimalTemplates;
using Savanna.Engine.GameMechanics.Models;
using Savanna.Engine.GameMechanics.Templates;
using Savanna.Engine.GameMechanics.Validators;
using System;

namespace Savanna.Engine.GameMechanics
{
    public class GenericMovement
    {
        private Random _rand = new Random();
        private CoordinateValidator _validator = new CoordinateValidator();

        public Coordinates Move(IField field, IFieldPresentable presentableObj, int stepsize)
        {
            var newCoords = GenerateNextMoveCoordinates(field, presentableObj, stepsize);
            field.Contents[presentableObj.CoordinateX, presentableObj.CoordinateY] = Settings.EmptyBlock;
            field.Contents[newCoords.CoordinateX, newCoords.CoordinateY] = presentableObj.Body;
            return newCoords;
        }

        private Coordinates GenerateNextMoveCoordinates(IField field, IFieldPresentable presentableObj, int stepsize)
        {
            int moveToX;
            int moveToY;
            do
            {
                do
                {
                    var offsetX = _rand.Next(-stepsize, ++stepsize);
                    var offsetY = _rand.Next(-stepsize, ++stepsize);
                    moveToX = presentableObj.CoordinateX + offsetX;
                    moveToY = presentableObj.CoordinateY + offsetY;
                } while (!_validator.CoordinatesAreValid(moveToX, moveToY));
            } while (field.Contents[moveToX, moveToY] != Settings.EmptyBlock);
            return new Coordinates(moveToX, moveToY);
        }
    }
}
