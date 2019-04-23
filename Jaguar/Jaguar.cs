using Savanna.Engine.Config;
using Savanna.Engine.GameMechanics;
using Savanna.Engine.GameMechanics.Animals.AnimalTemplates;
using Savanna.Engine.GameMechanics.Models;
using Savanna.Engine.GameMechanics.Templates;
using Savanna.Engine.GameMechanics.Validators;

namespace Jaguar
{
    public class Jaguar : ICarnivore
    {
        public int FieldOfView { get => 7; }
        public int StepSize { get => 3; }
        public char Body { get => 'J'; }
        public int CoordinateX { get; set; }
        public int CoordinateY { get; set; }

        private Movement _stdMove;
        private CoordinateValidator _validator;
        private PredatorEssentials _special;

        public Jaguar(Movement moves, CoordinateValidator validator, PredatorEssentials special)
        {
            _stdMove = moves;
            _validator = validator;
            _special = special;
        }

        public void Eat(IField field, Coordinates preyLocation)
        {
            field.Contents[preyLocation.CoordinateX, preyLocation.CoordinateY] = Body;
            field.Contents[CoordinateX, CoordinateY] = Settings.EmptyBlock;
            CoordinateX = preyLocation.CoordinateX;
            CoordinateY = preyLocation.CoordinateY;
        }

        public void Hunt(IField field, char preyBody)
        {
            var preyCoordinates = 
                _special.FindClosestPreyLocation(field, CoordinateX, CoordinateY, FieldOfView, preyBody);
            if (preyCoordinates != null)
            {
                if (_special.IsPreyInMoveRange(preyCoordinates, CoordinateX, CoordinateY, StepSize))
                {
                    Eat(field, preyCoordinates);
                }
                else
                {
                    var newPos =
                        _special.StalkPrey(preyCoordinates, field, CoordinateX, CoordinateY, StepSize, Body);
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
    }
}
