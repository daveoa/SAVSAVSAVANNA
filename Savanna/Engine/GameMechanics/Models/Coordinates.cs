namespace Savanna.Engine.GameMechanics.Models
{
    public class Coordinates
    {
        public int CoordinateX { get; set; }
        public int CoordinateY { get; set; }

        public Coordinates(int x, int y)
        {
            CoordinateX = x;
            CoordinateY = y;
        }
    }
}
