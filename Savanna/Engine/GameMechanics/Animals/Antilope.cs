using Savanna.Engine.Config;
using Savanna.Engine.GameMechanics.Animals.AnimalTemplates;
using Savanna.Engine.GameMechanics.Models;
using Savanna.Engine.GameMechanics.Templates;
using Savanna.Engine.GameMechanics.Validators;
using System;
using System.Collections.Generic;

namespace Savanna.Engine.GameMechanics.Animals
{
    public class Antilope : IHerbivore
    {
        public int FieldOfView { get => Settings.AntilopeSight; }
        public int StepSize { get => Settings.AntilopeStep; }
        public char Body { get => Settings.AntilopeBody; }
        public int CoordinateX { get; set; }
        public int CoordinateY { get; set; }

        private Movement _stdMove;
        private CoordinateValidator _validator;
        private PlacementCorrection _correct;

        public Antilope(Movement moves, CoordinateValidator validator, PlacementCorrection correct)
        {
            _stdMove = moves;
            _validator = validator;
            _correct = correct;
        }

        public void Evade(IField field)
        {
            var predatorLocations = GetAllNearbyPredators(field);
            if (predatorLocations != null)
            {
                var newPos = MoveAway(field, predatorLocations);
                CoordinateX = newPos.CoordinateX;
                CoordinateY = newPos.CoordinateY;
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

        private List<Coordinates> GetAllNearbyPredators(IField field)
        {
            var predatorsCoords = new List<Coordinates>();

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

        private Coordinates MoveAway(IField field, List<Coordinates> predatorsCoords)
        {
            var predatorAvg = GetAvgPredatorLocation(predatorsCoords);
            Coordinates newPos = GetMoveAwayPos(predatorAvg);
            if (field.Contents[newPos.CoordinateX, newPos.CoordinateY] != Settings.EmptyBlock)
            {
                newPos = 
                    _correct.CorrectFromStacking(field, newPos.CoordinateX, newPos.CoordinateY, CoordinateX, CoordinateY);
            }
            field.Contents[CoordinateX, CoordinateY] = Settings.EmptyBlock;
            field.Contents[newPos.CoordinateX, newPos.CoordinateY] = Body;

            return newPos;
        }

        private Coordinates GetAvgPredatorLocation(List<Coordinates> predatorsCoords)
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

        private Coordinates GetMoveAwayPos(Coordinates avgPredatorPos)
        {
            Coordinates newPos = null;
            if (IsInCorner())
            {
                newPos = MoveOutOfCorner(avgPredatorPos);
            }

            if (IsPinnedAgainstAWall())
            {
                newPos = MoveAlongWall(avgPredatorPos);
            }

            if (newPos == null)
            {
                var moveOffset = CalculateMoveAwayPos(avgPredatorPos);
                newPos = new Coordinates(CoordinateX + moveOffset.CoordinateX, CoordinateY + moveOffset.CoordinateY);
            }

            newPos.CoordinateX = _correct.AllignIfOutOfBounds(newPos.CoordinateX, FieldDimensions.Width);
            newPos.CoordinateY = _correct.AllignIfOutOfBounds(newPos.CoordinateY, FieldDimensions.Height);
            return newPos;
        }

        private bool IsPinnedAgainstAWall()
        {
            return CoordinateY == 0 || CoordinateX == 0 
               || CoordinateX == FieldDimensions.Width - 1 || CoordinateY == FieldDimensions.Height - 1;
        }

        private Coordinates MoveAlongWall(Coordinates avgPredatorPos)
        {
            int newXPos = this.CoordinateX;
            int newYPos = this.CoordinateY;

            newXPos += (CoordinateY == 0 && CoordinateX == avgPredatorPos.CoordinateX) ? StepSize : 0;
            newXPos += (CoordinateY == FieldDimensions.Height - 1 && CoordinateX == avgPredatorPos.CoordinateX) ? StepSize : 0;
            newYPos += (CoordinateY == 0 && CoordinateY == avgPredatorPos.CoordinateY) ? StepSize : 0;
            newYPos += (CoordinateY == FieldDimensions.Height - 1 && CoordinateY == avgPredatorPos.CoordinateY) ? -StepSize : 0;

            newYPos += (CoordinateX == 0 && CoordinateY == avgPredatorPos.CoordinateY) ? StepSize : 0;
            newYPos += (CoordinateX == FieldDimensions.Width - 1 && CoordinateY == avgPredatorPos.CoordinateY) ? StepSize : 0;
            newXPos += (CoordinateX == 0 && CoordinateX == avgPredatorPos.CoordinateX) ? StepSize : 0;
            newXPos += (CoordinateX == FieldDimensions.Width - 1 && CoordinateX == avgPredatorPos.CoordinateX) ? -StepSize : 0;

            if (newXPos == this.CoordinateX && newYPos == this.CoordinateY)
            {
                return null;
            }

            return new Coordinates(newXPos, newYPos);
        }

        private Coordinates MoveOutOfCorner(Coordinates avgPredatorPos)
        {
            int newXPos = this.CoordinateX;
            int newYPos = this.CoordinateY;
            int xDifference = this.CoordinateX - avgPredatorPos.CoordinateX;
            int yDifference = this.CoordinateY - avgPredatorPos.CoordinateY;

            if (xDifference < 0 && yDifference < 0)
            {
                newYPos += yDifference <= xDifference ? StepSize : 0;
                newXPos += yDifference > xDifference ? StepSize : 0;
            }
            if (xDifference >= 0 && yDifference < 0)
            {
                newYPos += Math.Abs(yDifference) <= xDifference ? StepSize : 0;
                newXPos += Math.Abs(yDifference) > xDifference ? -StepSize : 0;
            }
            if (xDifference < 0 && yDifference >= 0)
            {
                newYPos += yDifference <= Math.Abs(xDifference) ? -StepSize : 0;
                newXPos += yDifference > Math.Abs(xDifference) ? StepSize : 0;
            }
            if (xDifference >= 0 && yDifference >= 0)
            {
                newYPos += yDifference >= xDifference ? -StepSize : 0;
                newXPos += yDifference < xDifference ? -StepSize : 0;
            }

            return new Coordinates(newXPos, newYPos);
        }

        private bool IsInCorner()
        {
            return (this.CoordinateX == 0 || this.CoordinateX == FieldDimensions.Width - 1) 
                && (this.CoordinateY == 0 || this.CoordinateX == FieldDimensions.Height - 1);
        }

        private Coordinates CalculateMoveAwayPos(Coordinates avgPredatorPos)
        {
            int posXDifference = CoordinateX - avgPredatorPos.CoordinateX;
            int posYDifference = CoordinateY - avgPredatorPos.CoordinateY;

            if (Math.Abs(posXDifference) < StepSize && posYDifference == 0)
            {
                if (posXDifference < 0)
                {
                    return new Coordinates(-StepSize, 0);
                }
                else
                {
                    return new Coordinates(StepSize, 0);
                }
            }
            if (Math.Abs(posYDifference) < StepSize && posXDifference == 0)
            {
                if (posYDifference < 0)
                {
                    return new Coordinates(0, -StepSize);
                }
                else
                {
                    return new Coordinates(0, StepSize);
                }
            }

            while (Math.Abs(posXDifference) > StepSize || Math.Abs(posYDifference) > StepSize)
            {
                if (Math.Abs(posXDifference) > StepSize)
                {
                    posXDifference = MoveAwayAxisPointOneUnit(posXDifference);
                }
                if (Math.Abs(posYDifference) > StepSize)
                {
                    posYDifference = MoveAwayAxisPointOneUnit(posYDifference);
                }
            }

            while (Math.Abs(posXDifference) < StepSize && Math.Abs(posYDifference) < StepSize)
            {
                posXDifference += (posXDifference < 0) ? -1 : 1;
                posYDifference += (posYDifference < 0) ? -1 : 1;
            }
            return new Coordinates(posXDifference, posYDifference);
        }

        private int MoveAwayAxisPointOneUnit(int axisPointDifference)
        {
            if (Math.Abs(axisPointDifference) > StepSize && axisPointDifference < 0)
            {
                return ++axisPointDifference;
            }

            if (axisPointDifference > StepSize)
            {
                return --axisPointDifference;
            }

            return axisPointDifference;
        }
    }
}
