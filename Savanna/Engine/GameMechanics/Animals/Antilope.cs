using Savanna.Engine.Config;
using Savanna.Engine.GameMechanics.Animals.AnimalTemplates;
using Savanna.Engine.GameMechanics.Templates;

namespace Savanna.Engine.GameMechanics.Animals
{
    public class Antilope : Herbivore
    {
        public override int FieldOfView { get => Settings.AntilopeSight; }
        public override char Body { get => Settings.AntilopeBody; }

        private GenericMovement _generic = new GenericMovement();

        public override void Evade(IField field)
        {
            
        }

        public override void Move(IField field)
        {
            var newCoords = _generic.Move(field, this, this.FieldOfView);
            this.CoordinateX = newCoords.CoordinateX;
            this.CoordinateY = newCoords.CoordinateY;
        }
    }
}
