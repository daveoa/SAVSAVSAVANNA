using Savanna.Engine.Config;
using Savanna.Engine.GameMechanics.Animals.AnimalTemplates;
using Savanna.Engine.GameMechanics.Models;
using Savanna.Engine.GameMechanics.Templates;
using Savanna.Engine.GameMechanics.Validators;
using System.Collections.Generic;

namespace Savanna.Engine.GameMechanics.Animals
{
    public class Lion : Carnivore
    {
        public override int FieldOfView { get => Settings.LionSight; }
        public override int StepSize { get => Settings.LionStep; }
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
            var preyCoordinates = this.findClosestPreyLocation(field);
            if (preyCoordinates != null)
            {
                if (this.IsPreyInMoveRange(preyCoordinates))
                {
                    this.Eat(field, preyCoordinates);
                }
                else
                {
                    var newPos = this.MoveCloser(preyCoordinates, field);
                    this.CoordinateX = newPos.CoordinateX;
                    this.CoordinateY = newPos.CoordinateY;
                }
            }
            else
            {
                this.Move(field);
            }
        }

        public override void Move(IField field)
        {
            var newCoords = _generic.Move(field, this, this.StepSize);
            this.CoordinateX = newCoords.CoordinateX;
            this.CoordinateY = newCoords.CoordinateY;
        }

        private Coordinates findClosestPreyLocation(IField field)
        {
            var preyCoords = this.GetAllNearbyPrey(field);
            int closestIndex = 0;

            if (preyCoords.Count == 0)
            {
                return null;
            }
            for (int currentIndex = 0; currentIndex < preyCoords.Count; currentIndex++)
            {
                if (System.Math.Abs(preyCoords[currentIndex].CoordinateX + preyCoords[currentIndex].CoordinateY 
                    - this.CoordinateX + this.CoordinateY)
                    <
                    System.Math.Abs(preyCoords[closestIndex].CoordinateX + preyCoords[closestIndex].CoordinateY 
                    - this.CoordinateX + this.CoordinateY))
                {
                    closestIndex = currentIndex;
                }
            }
            return preyCoords[closestIndex];
        }

        private List<Coordinates> GetAllNearbyPrey(IField field)
        {
            var preyCoords = new List<Coordinates>();

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
                        preyCoords.Add(new Coordinates(xAxis, yAxis));
                    }
                }
            }

            return preyCoords;
        }

        private bool IsPreyInMoveRange(Coordinates preyLocation)
        {
            if (System.Math.Abs(preyLocation.CoordinateX - this.CoordinateX) > this.StepSize
                || System.Math.Abs(preyLocation.CoordinateY - this.CoordinateY) > this.StepSize)
            {
                return false;
            }
            return true;
        }

        private Coordinates MoveCloser(Coordinates preyCoords, IField field)
        {
            int newXPos;
            int newYPos;

            if (preyCoords.CoordinateX < this.CoordinateX)
            {
                newXPos = this.CoordinateX - this.StepSize;
            }
            else
            {
                newXPos = this.CoordinateX + this.StepSize;
            }

            if (preyCoords.CoordinateY < this.CoordinateY)
            {
                newYPos = this.CoordinateY - this.StepSize;
            }
            else
            {
                newYPos = this.CoordinateY + this.StepSize;
            }
            
            field.Contents[this.CoordinateX, this.CoordinateY] = Settings.EmptyBlock;
            field.Contents[newXPos, newYPos] = this.Body;
            return new Coordinates(newXPos, newYPos);
        }
    }
}
