using Savanna.Engine.Config;
using Savanna.Engine.GameMechanics.Animals.AnimalTemplates;
using Savanna.Engine.GameMechanics.Models;
using Savanna.Engine.GameMechanics.Templates;
using Savanna.Engine.GameMechanics.Validators;
using System;
using System.Collections.Generic;

namespace Savanna.Engine.GameMechanics.Animals
{
    public class Antilope : Herbivore
    {
        public override int FieldOfView { get => Settings.AntilopeSight; }
        public override int StepSize { get => Settings.AntilopeStep; }
        public override char Body { get => Settings.AntilopeBody; }

        private Movement _stdMove;
        private CoordinateValidator _validator;

        public Antilope(Movement moves, CoordinateValidator validator)
        {
            _stdMove = moves;
            _validator = validator;
        }

        public override void Evade(IField field)
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

        public override void Move(IField field)
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
            Coordinates newPos = DetermineMoveAway(predatorAvg);
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

        private Coordinates DetermineMoveAway(Coordinates avgPredatorPos)
        {
            int newXPos = CalculateMoveAwayAxisPoint(avgPredatorPos.CoordinateX, CoordinateX);
            int newYPos = CalculateMoveAwayAxisPoint(avgPredatorPos.CoordinateY, CoordinateY);

            newXPos = AllignIfOutOfBounds(newXPos, FieldDimensions.Width);
            newYPos = AllignIfOutOfBounds(newYPos, FieldDimensions.Height);

            return new Coordinates(newXPos, newYPos);
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

        private int CalculateMoveAwayAxisPoint(int avgPredatorAxisPoint, int preyAxisPoint)
        {
            int newAxisPoint = preyAxisPoint;

            if (avgPredatorAxisPoint > preyAxisPoint)
            {
                newAxisPoint -= StepSize;
            }
            if (avgPredatorAxisPoint < preyAxisPoint)
            {
                newAxisPoint += StepSize;
            }
            return newAxisPoint;
        }
    }
}
