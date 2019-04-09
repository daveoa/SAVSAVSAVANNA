using Savanna.Engine.Config;

namespace Savanna.Engine.GameMechanics.Validators
{
    public class CoordinateValidator
    {
        public bool CoordinatesAreValid(int x, int y)
        {
            if (!this.CoordinateXIsValid(x) || !this.CoordinateYIsValid(y))
            {
                return false;
            }
            return true;
        }

        public bool CoordinateXIsValid(int x)
        {
            if (x < 0 || x >= FieldDimensions.Width)
            {
                return false;
            }
            return true;
        }

        public bool CoordinateYIsValid(int y)
        {
            if (y < 0 || y >= FieldDimensions.Height)
            {
                return false;
            }
            return true;
        }
    }
}
