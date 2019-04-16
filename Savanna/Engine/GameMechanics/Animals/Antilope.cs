using Savanna.Engine.Config;
using Savanna.Engine.GameMechanics.Animals.AnimalTemplates;
using Savanna.Engine.GameMechanics.Templates;
using Savanna.Engine.GameMechanics.Validators;

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
        private PreyEssentials _special;
        private CoordinateValidator _validator;

        public Antilope(Movement moves, CoordinateValidator validator, PreyEssentials special)
        {
            _stdMove = moves;
            _validator = validator;
            _special = special;
        }

        public void Evade(IField field)
        {
            var predatorLocations = _special.GetAllNearbyPredators(field, this);
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
