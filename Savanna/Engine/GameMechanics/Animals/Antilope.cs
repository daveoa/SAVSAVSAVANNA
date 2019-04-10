using Savanna.Engine.Config;
using Savanna.Engine.GameMechanics.Animals.AnimalTemplates;
using Savanna.Engine.GameMechanics.Models;
using Savanna.Engine.GameMechanics.Templates;
using Savanna.Engine.GameMechanics.Validators;
using System.Collections.Generic;

namespace Savanna.Engine.GameMechanics.Animals
{
    public class Antilope : Herbivore
    {
        public override int FieldOfView { get => Settings.AntilopeSight; }
        public override int StepSize { get => Settings.AntilopeStep; }
        public override char Body { get => Settings.AntilopeBody; }

        private GenericMovement _generic = new GenericMovement();
        private CoordinateValidator _validator = new CoordinateValidator();
        private List<Coordinates> _predatorsInSight;

        public override void Evade(IField field)
        {
                this.Move(field);
        }

        public override void Move(IField field)
        {
            var newCoords = _generic.Move(field, this, this.StepSize);
            this.CoordinateX = newCoords.CoordinateX;
            this.CoordinateY = newCoords.CoordinateY;
        }

        private List<Coordinates> GetAllNearbyPredators(IField field)
        {
            var predatorsCoords = new List<Coordinates>();

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
                        predatorsCoords.Add(new Coordinates(xAxis, yAxis));
                    }
                }
            }

            if (predatorsCoords.Count == 0)
            {
                return null;
            }
            return predatorsCoords;
        }

        private Coordinates MoveAway(IField field, List<Coordinates> predatorsCoords)
        {
            // maybe think of something using weighted graphs, MAYBE
            return null;
        }
    }
}
