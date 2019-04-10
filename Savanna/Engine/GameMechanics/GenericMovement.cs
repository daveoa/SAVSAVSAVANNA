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

        public Coordinates Move(IField field, IFieldPresentable presentableObj, int fieldOfView)
        {
            var newCoords = GenerateNextMoveCoordinates(field, presentableObj, fieldOfView);
            field.Contents[presentableObj.CoordinateX, presentableObj.CoordinateY] = Settings.EmptyBlock;
            field.Contents[newCoords.CoordinateX, newCoords.CoordinateY] = presentableObj.Body;
            return newCoords;
        }

        private Coordinates GenerateNextMoveCoordinates(IField field, IFieldPresentable presentableObj, int fieldOfView)
        {
            int moveToX;
            int moveToY;
            do
            {
                do
                {
                    var offsetX = _rand.Next(-4, fieldOfView);
                    var offsetY = _rand.Next(-4, fieldOfView);
                    moveToX = presentableObj.CoordinateX + offsetX;
                    moveToY = presentableObj.CoordinateY + offsetY;
                } while (!_validator.CoordinatesAreValid(moveToX, moveToY));
            } while (field.Contents[moveToX, moveToY] != Settings.EmptyBlock);
            return new Coordinates(moveToX, moveToY);
        }
    }
}
