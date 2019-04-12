using Savanna.Engine.Config;
using Savanna.Engine.GameMechanics.Animals.AnimalTemplates;
using Savanna.Engine.GameMechanics.Models;
using Savanna.Engine.GameMechanics.Templates;
using Savanna.Engine.GameMechanics.Validators;
using System.Collections.Generic;

namespace Savanna.Engine.GameMechanics.Animals
{
    public class Lion : ICarnivore
    {
        public int FieldOfView { get => Settings.LionSight; }
        public int StepSize { get => Settings.LionStep; }
        public char Body { get => Settings.LionBody; }
        public int CoordinateX { get; set; }
        public int CoordinateY { get; set; }

        private Movement _stdMove;
        private CoordinateValidator _validator;

        public Lion(Movement moves, CoordinateValidator validator)
        {
            _stdMove = moves;
            _validator = validator;
        }

        public void Eat(IField field, Coordinates preyLocation)
        {
            field.Contents[preyLocation.CoordinateX, preyLocation.CoordinateY] = Body;
            field.Contents[CoordinateX, CoordinateY] = Settings.EmptyBlock;
            CoordinateX = preyLocation.CoordinateX;
            CoordinateY = preyLocation.CoordinateY;
        }

        public void Hunt(IField field)
        {
            var preyCoordinates = FindClosestPreyLocation(field);
            if (preyCoordinates != null)
            {
                if (IsPreyInMoveRange(preyCoordinates))
                {
                    Eat(field, preyCoordinates);
                }
                else
                {
                    var newPos = MoveCloser(preyCoordinates, field);
                    CoordinateX = newPos.CoordinateX;
                    CoordinateY = newPos.CoordinateY;
                }
            }
            else
            {
                Move(field);
            }
        }

        public void Move(IField field)
        {
            var newCoords = _stdMove.Move(field, this, StepSize);
            CoordinateX = newCoords.CoordinateX;
            CoordinateY = newCoords.CoordinateY;
        }

        private Coordinates FindClosestPreyLocation(IField field)
        {
            var preyCoords = GetAllNearbyPrey(field);
            int closestIndex = 0;

            if (preyCoords.Count == 0)
            {
                return null;
            }
            for (int currentIndex = 0; currentIndex < preyCoords.Count; currentIndex++)
            {
                if (System.Math.Abs(preyCoords[currentIndex].CoordinateX + preyCoords[currentIndex].CoordinateY 
                    - CoordinateX + CoordinateY)
                    <
                    System.Math.Abs(preyCoords[closestIndex].CoordinateX + preyCoords[closestIndex].CoordinateY 
                    - CoordinateX + CoordinateY))
                {
                    closestIndex = currentIndex;
                }
            }
            return preyCoords[closestIndex];
        }

        private List<Coordinates> GetAllNearbyPrey(IField field)
        {
            var preyCoords = new List<Coordinates>();

            for (int yAxis = CoordinateY - FieldOfView; yAxis <= CoordinateY + FieldOfView; yAxis++)
            {
                if (!_validator.CoordinateYIsValid(yAxis))
                {
                    continue;
                }
                for (int xAxis = CoordinateX - FieldOfView; xAxis <= CoordinateX + FieldOfView; xAxis++)
                {
                    if (!_validator.CoordinateXIsValid(xAxis))
                    {
                        continue;
                    }
                    if (field.Contents[xAxis, yAxis] == Settings.AntilopeBody)
                    {
                        preyCoords.Add(new Coordinates(xAxis, yAxis));
                    }
                }
            }

            return preyCoords;
        }

        private bool IsPreyInMoveRange(Coordinates preyLocation)
        {
            if (System.Math.Abs(preyLocation.CoordinateX - CoordinateX) > StepSize
                || System.Math.Abs(preyLocation.CoordinateY - CoordinateY) > StepSize)
            {
                return false;
            }
            return true;
        }

        private Coordinates MoveCloser(Coordinates preyCoords, IField field)
        {
            int newXPos;
            int newYPos;
            int changeableStep = StepSize;

            do
            {
                newXPos = CalculateMovingInPoint(preyCoords.CoordinateX, CoordinateX, changeableStep);
                --changeableStep;
            } while ((newXPos < 0) || (newXPos >= FieldDimensions.Width));

            changeableStep = StepSize;
            do
            {
                newYPos = CalculateMovingInPoint(preyCoords.CoordinateY, CoordinateY, changeableStep);
                --changeableStep;
            } while ((newYPos < 0) || (newYPos >= FieldDimensions.Height));

            field.Contents[CoordinateX, CoordinateY] = Settings.EmptyBlock;
            field.Contents[newXPos, newYPos] = Body;
            return new Coordinates(newXPos, newYPos);
        }

        private int CalculateMovingInPoint(int preyAxisPoint, int hunterAxisPoint, int stepSize)
        {
            int newPoint = hunterAxisPoint;
            int changeableStepSize = stepSize;
            if (preyAxisPoint < hunterAxisPoint)
            {
                newPoint = hunterAxisPoint - changeableStepSize;
            }
            else if (preyAxisPoint > hunterAxisPoint)
            {
                newPoint = hunterAxisPoint + changeableStepSize;
            }
            return newPoint;
        }
    }
}
