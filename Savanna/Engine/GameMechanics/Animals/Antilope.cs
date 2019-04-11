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

        private Movement _stdMove;
        private CoordinateValidator _validator;

        public Antilope(Movement moves, CoordinateValidator validator)
        {
            _stdMove = moves;
            _validator = validator;
        }

        public override void Evade(IField field)
        {
                Move(field);
        }

        public override void Move(IField field)
        {
            var newCoords = _stdMove.Move(field, this, StepSize);
            CoordinateX = newCoords.CoordinateX;
            CoordinateY = newCoords.CoordinateY;
        }

        private List<Coordinates> GetAllNearbyPredators(IField field)
        {
            var predatorsCoords = new List<Coordinates>();

            for (int yAxis = CoordinateY - FieldOfView; yAxis <= CoordinateY + FieldOfView; yAxis++)
            {
                if (!_validator.CoordinateYIsValid(yAxis))
                {
                    continue;
                }
                for (int xAxis = CoordinateX - FieldOfView; xAxis <= CoordinateX + FieldOfView; xAxis++)
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
