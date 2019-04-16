using Savanna.Engine.Config;
using Savanna.Engine.GameMechanics.Models;
using Savanna.Engine.GameMechanics.Templates;

namespace Savanna.Engine.GameMechanics
{
    public class PlacementCorrection
    {
        public Coordinates CorrectFromStacking(IField field, int currentX, int currentY, int originX, int originY)
        {
            if (currentX > originX && currentY > originY)
            {
                for (int height = currentY; height >= originY; height--)
                {
                    for (int width = currentX; width >= originX; width--)
                    {
                        if (field.Contents[width, height] == Settings.EmptyBlock)
                        {
                            return new Coordinates(width, height);
                        }
                    }
                }
            }
            if (currentX < originX && currentY > originY)
            {
                for (int height = currentY; height >= originY; height--)
                {
                    for (int width = currentX; width <= originX; width++)
                    {
                        if (field.Contents[width, height] == Settings.EmptyBlock)
                        {
                            return new Coordinates(width, height);
                        }
                    }
                }
            }
            if (currentX > originX && currentY < originY)
            {
                for (int height = currentY; height <= originY; height++)
                {
                    for (int width = currentX; width >= originX; width--)
                    {
                        if (field.Contents[width, height] == Settings.EmptyBlock)
                        {
                            return new Coordinates(width, height);
                        }
                    }
                }
            }
            if (currentX < originX && currentY < originY)
            {
                for (int height = currentY; height <= originY; height++)
                {
                    for (int width = currentX; width <= originX; width++)
                    {
                        if (field.Contents[width, height] == Settings.EmptyBlock)
                        {
                            return new Coordinates(width, height);
                        }
                    }
                }
            }

            if (currentX < originX && currentY == originY)
            {
                for (int width = currentX; width <= originX; width++)
                {
                    if (field.Contents[width, originY] == Settings.EmptyBlock)
                    {
                        return new Coordinates(width, originY);
                    }
                }
            }
            if (currentX > originX && currentY == originY)
            {
                for (int width = currentX; width >= originX; width--)
                {
                    if (field.Contents[width, originY] == Settings.EmptyBlock)
                    {
                        return new Coordinates(width, originY);
                    }
                }
            }
            if (currentX == originX && currentY > originY)
            {
                for (int height = currentY; height >= originY; height--)
                {
                    if (field.Contents[originX, height] == Settings.EmptyBlock)
                    {
                        return new Coordinates(originX, height);
                    }
                }
            }
            if (currentX == originX && currentY < originY)
            {
                for (int height = currentY; height <= originY; height++)
                {
                    if (field.Contents[originX, height] == Settings.EmptyBlock)
                    {
                        return new Coordinates(originX, height);
                    }
                }
            }

            return new Coordinates(originX, originY);
        }

        public int AllignIfOutOfBounds(int axisPoint, int maxPoint)
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
    }
}
