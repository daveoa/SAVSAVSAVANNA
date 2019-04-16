using Savanna.Engine.Config;
using Savanna.Engine.GameMechanics.Animals.AnimalTemplates;
using Savanna.Engine.GameMechanics.Models;
using Savanna.Engine.GameMechanics.Templates;
using Savanna.Engine.GameMechanics.Validators;
using System;
using System.Collections.Generic;

namespace Savanna.Engine.GameMechanics
{
    public class PreyEssentials
    {
        private CoordinateValidator _validator;
        private PlacementCorrection _correct;

        public PreyEssentials(CoordinateValidator validator, PlacementCorrection correct)
        {
            _validator = validator;
            _correct = correct;
        }

        public List<Coordinates> GetAllNearbyPredators(IField field, IHerbivore prey)
        {
            var predatorsCoords = new List<Coordinates>();

            for (int yAxis = prey.CoordinateY - prey.FieldOfView; yAxis <= prey.CoordinateY + prey.FieldOfView; yAxis++)
            {
                if (!_validator.CoordinateYIsValid(yAxis))
                {
                    continue;
                }
                for (int xAxis = prey.CoordinateX - prey.FieldOfView; xAxis <= prey.CoordinateX + prey.FieldOfView; xAxis++)
                {
                    if (!_validator.CoordinateXIsValid(xAxis))
                    {
                        continue;
                    }
                    if (field.Contents[xAxis, yAxis] == Settings.LionBody)
                    {
                        predatorsCoords.Add(new Coordinates(xAxis, yAxis));
                    }
                }
            }

            if (predatorsCoords.Count == 0)
            {
                return null;
            }
            return predatorsCoords;
        }

        public Coordinates MoveAway(IField field, List<Coordinates> predatorsCoords, IHerbivore prey)
        {
            var predatorAvg = GetAvgPredatorLocation(predatorsCoords);
            Coordinates newPos = GetMoveAwayPos(predatorAvg, prey);
            if (field.Contents[newPos.CoordinateX, newPos.CoordinateY] != Settings.EmptyBlock)
            {
                newPos = _correct.CorrectFromStacking
                    (field, newPos.CoordinateX, newPos.CoordinateY, prey.CoordinateX, prey.CoordinateY);
            }

            field.Contents[prey.CoordinateX, prey.CoordinateY] = Settings.EmptyBlock;
            field.Contents[newPos.CoordinateX, newPos.CoordinateY] = prey.Body;

            return newPos;
        }

        public Coordinates GetAvgPredatorLocation(List<Coordinates> predatorsCoords)
        {
            double avgXPos = 0, avgYPos = 0;

            foreach (var pair in predatorsCoords)
            {
                avgXPos += pair.CoordinateX;
                avgYPos += pair.CoordinateY;
            }
            avgXPos = Math.Round(avgXPos / predatorsCoords.Count);
            avgYPos = Math.Round(avgYPos / predatorsCoords.Count);

            return new Coordinates((int)avgXPos, (int)avgYPos);
        }

        private Coordinates GetMoveAwayPos(Coordinates avgPredatorPos, IHerbivore prey)
        {
            Coordinates newPos = null;
            if (IsInCorner(prey.CoordinateX, prey.CoordinateY))
            {
                newPos = MoveOutOfCorner(avgPredatorPos, prey.CoordinateX, prey.CoordinateY, prey.StepSize);
            }

            if (IsPinnedAgainstAWall(prey.CoordinateX, prey.CoordinateY))
            {
                newPos = MoveAlongWall(avgPredatorPos, prey.CoordinateX, prey.CoordinateY, prey.StepSize);
            }

            if (newPos == null)
            {
                var moveOffset = CalculateMoveAwayPos(avgPredatorPos, prey);
                newPos = new Coordinates(prey.CoordinateX + moveOffset.CoordinateX,
                                         prey.CoordinateY + moveOffset.CoordinateY);
            }

            newPos.CoordinateX = _correct.AllignIfOutOfBounds(newPos.CoordinateX, FieldDimensions.Width);
            newPos.CoordinateY = _correct.AllignIfOutOfBounds(newPos.CoordinateY, FieldDimensions.Height);
            return newPos;
        }

        public bool IsPinnedAgainstAWall(int preyX, int preyY)
        {
            return preyY == 0 || preyX == 0
               || preyX == FieldDimensions.Width - 1 || preyY == FieldDimensions.Height - 1;
        }

        private Coordinates MoveAlongWall(Coordinates avgPredatorPos, int preyX, int preyY, int stepSize)
        {
            int newXPos = preyX;
            int newYPos = preyY;

            newXPos += (preyY == 0 && preyX == avgPredatorPos.CoordinateX) ? stepSize : 0;
            newXPos += (preyY == FieldDimensions.Height - 1 && preyX == avgPredatorPos.CoordinateX) ? stepSize : 0;
            newYPos += (preyY == 0 && preyY == avgPredatorPos.CoordinateY) ? stepSize : 0;
            newYPos += (preyY == FieldDimensions.Height - 1 && preyY == avgPredatorPos.CoordinateY) ? -stepSize : 0;

            newYPos += (preyX == 0 && preyY == avgPredatorPos.CoordinateY) ? stepSize : 0;
            newYPos += (preyX == FieldDimensions.Width - 1 && preyY == avgPredatorPos.CoordinateY) ? stepSize : 0;
            newXPos += (preyX == 0 && preyX == avgPredatorPos.CoordinateX) ? stepSize : 0;
            newXPos += (preyX == FieldDimensions.Width - 1 && preyX == avgPredatorPos.CoordinateX) ? -stepSize : 0;

            if (newXPos == preyX && newYPos == preyY)
            {
                return null;
            }

            return new Coordinates(newXPos, newYPos);
        }

        private Coordinates MoveOutOfCorner(Coordinates avgPredatorPos, int preyX, int preyY, int stepSize)
        {
            int newXPos = preyX;
            int newYPos = preyY;
            int xDifference = preyX - avgPredatorPos.CoordinateX;
            int yDifference = preyY - avgPredatorPos.CoordinateY;

            if (xDifference < 0 && yDifference < 0)
            {
                newYPos += yDifference <= xDifference ? stepSize : 0;
                newXPos += yDifference > xDifference ? stepSize : 0;
            }
            if (xDifference >= 0 && yDifference < 0)
            {
                newYPos += Math.Abs(yDifference) <= xDifference ? stepSize : 0;
                newXPos += Math.Abs(yDifference) > xDifference ? -stepSize : 0;
            }
            if (xDifference < 0 && yDifference >= 0)
            {
                newYPos += yDifference <= Math.Abs(xDifference) ? -stepSize : 0;
                newXPos += yDifference > Math.Abs(xDifference) ? stepSize : 0;
            }
            if (xDifference >= 0 && yDifference >= 0)
            {
                newYPos += yDifference >= xDifference ? -stepSize : 0;
                newXPos += yDifference < xDifference ? -stepSize : 0;
            }

            return new Coordinates(newXPos, newYPos);
        }

        public bool IsInCorner(int preyX, int preyY)
        {
            return (preyX == 0 || preyX == FieldDimensions.Width - 1)
                && (preyY == 0 || preyX == FieldDimensions.Height - 1);
        }

        private Coordinates CalculateMoveAwayPos(Coordinates avgPredatorPos, IHerbivore prey)
        {
            int posXDifference = prey.CoordinateX - avgPredatorPos.CoordinateX;
            int posYDifference = prey.CoordinateY - avgPredatorPos.CoordinateY;

            if (Math.Abs(posXDifference) < prey.StepSize && posYDifference == 0)
            {
                if (posXDifference < 0)
                {
                    return new Coordinates(-prey.StepSize, 0);
                }
                else
                {
                    return new Coordinates(prey.StepSize, 0);
                }
            }
            if (Math.Abs(posYDifference) < prey.StepSize && posXDifference == 0)
            {
                if (posYDifference < 0)
                {
                    return new Coordinates(0, -prey.StepSize);
                }
                else
                {
                    return new Coordinates(0, prey.StepSize);
                }
            }

            while (Math.Abs(posXDifference) > prey.StepSize || Math.Abs(posYDifference) > prey.StepSize)
            {
                if (Math.Abs(posXDifference) > prey.StepSize)
                {
                    posXDifference = MoveAwayAxisPointOneUnit(posXDifference, prey.StepSize);
                }
                if (Math.Abs(posYDifference) > prey.StepSize)
                {
                    posYDifference = MoveAwayAxisPointOneUnit(posYDifference, prey.StepSize);
                }
            }

            while (Math.Abs(posXDifference) < prey.StepSize && Math.Abs(posYDifference) < prey.StepSize)
            {
                posXDifference += (posXDifference < 0) ? -1 : 1;
                posYDifference += (posYDifference < 0) ? -1 : 1;
            }
            return new Coordinates(posXDifference, posYDifference);
        }

        private int MoveAwayAxisPointOneUnit(int axisPointDifference, int stepSize)
        {
            if (Math.Abs(axisPointDifference) > stepSize && axisPointDifference < 0)
            {
                return ++axisPointDifference;
            }

            if (axisPointDifference > stepSize)
            {
                return --axisPointDifference;
            }

            return axisPointDifference;
        }
    }
}
