using System;

namespace Savanna.Engine.GameMechanics
{
    public class AxisPointCalculations
    {
        public int CalculateMovingInPoint(int preyAxisPoint, int hunterAxisPoint, int stepSize)
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

        public int CalculateDistancePrey(int preyX, int preyY, int predatorX, int predatorY)
        {
            return System.Math.Abs(preyX + preyY - predatorX - predatorY);
        }

        public int CorrectMovingInPoint(int preyAxisPoint, int predatorAxisPoint, int predatorStepSize, int maxFieldDimension)
        {
            int newAxisPoint;
            int changeableStep = predatorStepSize;

            do
            {
                newAxisPoint = CalculateMovingInPoint(preyAxisPoint, predatorAxisPoint, changeableStep);
                --changeableStep;
            } while ((newAxisPoint < 0) || (newAxisPoint >= maxFieldDimension));

            return newAxisPoint;
        }
    }
}
