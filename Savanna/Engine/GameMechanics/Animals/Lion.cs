using Savanna.Engine.Config;
using Savanna.Engine.GameMechanics.Animals.AnimalTemplates;
using Savanna.Engine.GameMechanics.Models;
using Savanna.Engine.GameMechanics.Templates;
using Savanna.Engine.GameMechanics.Validators;

namespace Savanna.Engine.GameMechanics.Animals
{
    public class Lion : Carnivore
    {
        public override int FieldOfView { get => Settings.LionSight; }
        public override char Body { get => Settings.LionBody; }

        private GenericMovement _generic = new GenericMovement();
        private CoordinateValidator _validator = new CoordinateValidator();

        public override void Eat(IField field, Coordinates preyLocation)
        {
            field.Contents[preyLocation.CoordinateX, preyLocation.CoordinateY] = this.Body;
            field.Contents[this.CoordinateX, this.CoordinateY] = Settings.EmptyBlock;
            this.CoordinateX = preyLocation.CoordinateX;
            this.CoordinateY = preyLocation.CoordinateY;
        }

        public override void Hunt(IField field)
        {
            var preyCoordinates = this.findPreyLocation(field);
            if (preyCoordinates != null)
            {
                this.Eat(field, preyCoordinates);
                return;
            }
            this.Move(field);
        }

        public override void Move(IField field)
        {
            var newCoords = _generic.Move(field, this, this.FieldOfView);
            this.CoordinateX = newCoords.CoordinateX;
            this.CoordinateY = newCoords.CoordinateY;
        }

        private Coordinates findPreyLocation(IField field)
        {
            for (int yAxis = this.CoordinateY - this.FieldOfView; yAxis <= this.CoordinateY + this.FieldOfView; yAxis++)
            {
                if (!_validator.CoordinateYIsValid(yAxis))
                {
                    continue;
                }
                for (int xAxis = this.CoordinateX - this.FieldOfView; xAxis <= this.CoordinateX + this.FieldOfView; xAxis++)
                {
                    if (!_validator.CoordinateXIsValid(xAxis))
                    {
                        continue;
                    }
                    if (field.Contents[xAxis, yAxis] == Settings.AntilopeBody)
                    {
                        var preyCoords = new Coordinates(xAxis, yAxis);
                        return preyCoords;
                    }
                }
            }
            return null;
        }
    }
}
