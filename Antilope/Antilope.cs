using Savanna.Engine.Config;
using Savanna.Engine.GameMechanics;
using Savanna.Engine.GameMechanics.Animals.AnimalTemplates;
using Savanna.Engine.GameMechanics.Templates;
using Savanna.Engine.GameMechanics.Validators;

namespace Antilope
{
    public class Antilope : IHerbivore
    {
        public int FieldOfView { get => 4; }
        public int StepSize { get => 3; }
        public char Body { get => 'A'; }
        public int CoordinateX { get; set; }
        public int CoordinateY { get; set; }

        private Movement _stdMove;
        private PreyEssentials _special;
        private CoordinateValidator _validator;

        public Antilope(Movement moves, CoordinateValidator validator, PreyEssentials special)
        {
            _stdMove = moves;
            _validator = validator;
            _special = special;
        }

        public void Evade(IField field, char predatorBody)
        {
            var predatorLocations = _special.GetAllNearbyPredators(field, this, predatorBody);
            if (predatorLocations != null)
            {
                var newPos = _special.MoveAway(field, predatorLocations, this);
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
    }
}
