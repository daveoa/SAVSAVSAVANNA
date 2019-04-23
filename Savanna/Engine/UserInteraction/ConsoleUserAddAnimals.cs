using Savanna.Engine.Config;
using Savanna.Engine.GameMechanics;
using Savanna.Engine.GameMechanics.Animals;
using Savanna.Engine.GameMechanics.Animals.AnimalTemplates;
using Savanna.Engine.GameMechanics.Models;
using Savanna.Engine.GameMechanics.Templates;
using Savanna.Engine.GameMechanics.Validators;
using System;
using System.Collections.Generic;

namespace Savanna.Engine.UserInteraction
{
    public class ConsoleUserAddAnimals
    {
        private Movement _standardMovement;
        private CoordinateValidator _validator;
        private PredatorEssentials _predatorSpecial;
        private PreyEssentials _preySpecial;
        private ISpawner _spawner;

        public ConsoleUserAddAnimals
            (Movement standardMovement, CoordinateValidator validator, PredatorEssentials predatorSpecial,
            PreyEssentials preySpecial, Spawner spawner)
        {
            _standardMovement = standardMovement;
            _validator = validator;
            _predatorSpecial = predatorSpecial;
            _preySpecial = preySpecial;
            _spawner = spawner;
        }

        public AnimalLists AddAnimals(AnimalLists animalLists, IField field, Dictionary<char, Type> bodyAndType)
        {
            Console.WriteLine(GetNotification(bodyAndType));
            char key;
            while (Console.KeyAvailable)
            {
                key = Console.ReadKey(true).KeyChar;
                var newAnimalType = GetAnimalTypeFromKeyPress(key, bodyAndType);
                if (newAnimalType != default(Type))
                {
                    if (typeof(IHerbivore).IsAssignableFrom(newAnimalType))
                    {
                        var animal = (IHerbivore)Activator.CreateInstance(newAnimalType, _standardMovement, _validator, _preySpecial);
                        _spawner.SetSpawnPoint(field, animal);
                        animalLists.Prey.Add(animal);
                    }
                    else if (typeof(ICarnivore).IsAssignableFrom(newAnimalType))
                    {
                        var animal = (ICarnivore)Activator.CreateInstance(newAnimalType, _standardMovement, _validator, _predatorSpecial);
                        _spawner.SetSpawnPoint(field, animal);
                        animalLists.Hunters.Add(animal);
                    }
                }
            }
            return animalLists;
        }

        private string GetNotification(Dictionary<char, Type> bodyAndType)
        {
            string notification = "";
            foreach (var item in bodyAndType)
            {
                notification += item.Key + " - " + item.Value.Name + " ";
            }
            return notification;
        }

        private Type GetAnimalTypeFromKeyPress(char key, Dictionary<char, Type> bodyAndType)
        {
            foreach (var item in bodyAndType)
            {
                if (key == Char.ToLower(item.Key))
                {
                    return item.Value;
                }
            }
            return default(Type);
        }
    }
}
