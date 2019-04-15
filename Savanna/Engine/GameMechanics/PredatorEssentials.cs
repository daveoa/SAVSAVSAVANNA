using Savanna.Engine.Config;
using Savanna.Engine.GameMechanics.Models;
using Savanna.Engine.GameMechanics.Templates;
using Savanna.Engine.GameMechanics.Validators;
using System;
using System.Collections.Generic;

namespace Savanna.Engine.GameMechanics
{
    public class PredatorEssentials
    {
        private CoordinateValidator _validator;
        private AxisPointCalculations _pointCalc;
        private PlacementCorrection _correct;

        public PredatorEssentials
            (CoordinateValidator validator, AxisPointCalculations pointCalc, PlacementCorrection correct)
        {
            _validator = validator;
            _pointCalc = pointCalc;
            _correct = correct;
        }

        public List<Coordinates> GetAllNearbyPrey(IField field, int predatorX, int predatorY, int predatorFOV)
        {
            var preyCoords = new List<Coordinates>();

            for (int yAxis = predatorY - predatorFOV; yAxis <= predatorY + predatorFOV; yAxis++)
            {
                if (!_validator.CoordinateYIsValid(yAxis))
                {
                    continue;
                }
                for (int xAxis = predatorX - predatorFOV; xAxis <= predatorX + predatorFOV; xAxis++)
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

        public Coordinates FindClosestPreyLocation(IField field, int predatorX, int predatorY, int predatorFOV)
        {
            var preyCoords = GetAllNearbyPrey(field, predatorX, predatorY, predatorFOV);
            int closestIndex = 0;

            if (preyCoords.Count == 0)
            {
                return null;
            }
            for (int currentIndex = 0; currentIndex < preyCoords.Count; currentIndex++)
            {
                var firstPreyDistance = _pointCalc.CalculateDistancePrey(preyCoords[currentIndex].CoordinateX,
                    preyCoords[currentIndex].CoordinateY, predatorX, predatorY);

                var secondPreyDistance = _pointCalc.CalculateDistancePrey(preyCoords[closestIndex].CoordinateX,
                    preyCoords[closestIndex].CoordinateY, predatorX, predatorY);

                if (firstPreyDistance < secondPreyDistance)
                {
                    closestIndex = currentIndex;
                }
            }
            return preyCoords[closestIndex];
        }

        public bool IsPreyInMoveRange(Coordinates preyLocation, int predatorX, int predatorY, int stepSize)
        {
            return (Math.Abs(preyLocation.CoordinateX - predatorX) <= stepSize)
                && (Math.Abs(preyLocation.CoordinateY - predatorY) <= stepSize);
        }

        public Coordinates StalkPrey
            (Coordinates preyCoords, IField field, int predatorX, int predatorY, int stepSize, char body)
        {
            int newXPos = _pointCalc.CorrectMovingInPoint(preyCoords.CoordinateX, predatorX, stepSize, FieldDimensions.Width);
            int newYPos = _pointCalc.CorrectMovingInPoint(preyCoords.CoordinateY, predatorY, stepSize, FieldDimensions.Height);
            var newPos = new Coordinates(newXPos, newYPos);

            if (field.Contents[newXPos, newYPos] != Settings.EmptyBlock)
            {
                newPos = _correct.CorrectFromStacking(field, newXPos, newYPos, predatorX, predatorY);
            }

            field.Contents[predatorX, predatorY] = Settings.EmptyBlock;
            field.Contents[newPos.CoordinateX, newPos.CoordinateY] = body;
            return newPos;
        }
    }
}
