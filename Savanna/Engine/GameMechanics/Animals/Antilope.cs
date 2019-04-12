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

        public Antilope(Movement moves, CoordinateValidator validator)
        {
            _stdMove = moves;
            _validator = validator;
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
                newPos = CorrectFromStacking(field, newPos);
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

            var moveOffset = CalculateMoveAwayPos(avgPredatorPos);
            int newXPos = CoordinateX + moveOffset.CoordinateX;
            int newYPos = CoordinateY + moveOffset.CoordinateY;

            newXPos = AllignIfOutOfBounds(newXPos, FieldDimensions.Width);
            newYPos = AllignIfOutOfBounds(newYPos, FieldDimensions.Height);

            return new Coordinates(newXPos, newYPos);
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

        private int AllignIfOutOfBounds(int axisPoint, int maxPoint)
        {
            if (axisPoint < 0)
            {
                return 0;
            }
            if (axisPoint >= maxPoint)
            {
                axisPoint = maxPoint - 1;
                return axisPoint;
            }
            return axisPoint;
        }

        private Coordinates CorrectFromStacking(IField field, Coordinates currentPos)
        {
            if (currentPos.CoordinateX > CoordinateX && currentPos.CoordinateY > CoordinateY)
            {
                for (int height = currentPos.CoordinateY; height >= CoordinateY; height--)
                {
                    for (int width = currentPos.CoordinateX; width >= CoordinateX; width--)
                    {
                        if (field.Contents[width, height] == Settings.EmptyBlock)
                        {
                            return new Coordinates(width, height);
                        }
                    }
                }
            }
            if (currentPos.CoordinateX < CoordinateX && currentPos.CoordinateY > CoordinateY)
            {
                for (int height = currentPos.CoordinateY; height >= CoordinateY; height--)
                {
                    for (int width = currentPos.CoordinateX; width <= CoordinateX; width++)
                    {
                        if (field.Contents[width, height] == Settings.EmptyBlock)
                        {
                            return new Coordinates(width, height);
                        }
                    }
                }
            }
            if (currentPos.CoordinateX > CoordinateX && currentPos.CoordinateY < CoordinateY)
            {
                for (int height = currentPos.CoordinateY; height <= CoordinateY; height++)
                {
                    for (int width = currentPos.CoordinateX; width >= CoordinateX; width--)
                    {
                        if (field.Contents[width, height] == Settings.EmptyBlock)
                        {
                            return new Coordinates(width, height);
                        }
                    }
                }
            }
            if (currentPos.CoordinateX < CoordinateX && currentPos.CoordinateY < CoordinateY)
            {
                for (int height = currentPos.CoordinateY; height <= CoordinateY; height++)
                {
                    for (int width = currentPos.CoordinateX; width <= CoordinateX; width++)
                    {
                        if (field.Contents[width, height] == Settings.EmptyBlock)
                        {
                            return new Coordinates(width, height);
                        }
                    }
                }
            }

            if (currentPos.CoordinateX < CoordinateX && currentPos.CoordinateY == CoordinateY)
            {
                for (int width = currentPos.CoordinateX; width <= CoordinateX; width++)
                {
                    if (field.Contents[width, CoordinateY] == Settings.EmptyBlock)
                    {
                        return new Coordinates(width, CoordinateY);
                    }
                }
            }
            if (currentPos.CoordinateX > CoordinateX && currentPos.CoordinateY == CoordinateY)
            {
                for (int width = currentPos.CoordinateX; width >= CoordinateX; width--)
                {
                    if (field.Contents[width, CoordinateY] == Settings.EmptyBlock)
                    {
                        return new Coordinates(width, CoordinateY);
                    }
                }
            }
            if (currentPos.CoordinateX == CoordinateX && currentPos.CoordinateY > CoordinateY)
            {
                for (int height = currentPos.CoordinateY; height >= CoordinateY; height--)
                {
                    if (field.Contents[CoordinateX, height] == Settings.EmptyBlock)
                    {
                        return new Coordinates(CoordinateX, height);
                    }
                }
            }
            if (currentPos.CoordinateX == CoordinateX && currentPos.CoordinateY < CoordinateY)
            {
                for (int height = currentPos.CoordinateY; height <= CoordinateY; height++)
                {
                    if (field.Contents[CoordinateX, height] == Settings.EmptyBlock)
                    {
                        return new Coordinates(CoordinateX, height);
                    }
                }
            }
            
            return new Coordinates(CoordinateX, CoordinateY);
        }
    }
}
