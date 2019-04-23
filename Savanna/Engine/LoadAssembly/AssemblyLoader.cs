using Savanna.Engine.GameMechanics;
using Savanna.Engine.GameMechanics.Animals.AnimalTemplates;
using Savanna.Engine.GameMechanics.Validators;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Savanna.Engine.LoadAssembly
{
    public class AssemblyLoader
    {
        public string BinPath;
        List<Assembly> AllAssemblies;
        CoordinateValidator _validator;
        Movement _movement;
        PreyEssentials _preySpecial;
        PredatorEssentials _predatorSpecial;

        public AssemblyLoader
            (CoordinateValidator validator, Movement movement, PreyEssentials preySpecial, PredatorEssentials predatorSpecial)
        {
            _validator = validator;
            _movement = movement;
            _predatorSpecial = predatorSpecial;
            _preySpecial = preySpecial;
            AllAssemblies = new List<Assembly>();
            BinPath = @"C:\Users\deivs.oskars.alksnis\source\repos\Savanna\Savanna\bin";
            foreach (string dll in Directory.GetFiles(BinPath, "*.dll"))
            {
                AllAssemblies.Add(Assembly.LoadFile(dll));
            }
        }

        public List<string> RetrieveAnimalNames()
        {
            var typeList = new List<string>();
            foreach (var assembly in AllAssemblies)
            {
                foreach (var types in assembly.GetTypes())
                {
                    typeList.Add(types.FullName);
                }
            }
            return typeList;
        }

        public Dictionary<char, Type> RetrieveAnimalBodies()
        {
            var bodyDictionary = new Dictionary<char, Type>();
            int assemblyIndex = 0;
            var allAnimalClassNames = RetrieveAnimalNames();

            foreach (var className in allAnimalClassNames)
            {
                var type = AllAssemblies[assemblyIndex].GetType(className);
                IFieldPresentable animal;
                if (typeof(IHerbivore).IsAssignableFrom(type))
                {
                    animal = (IFieldPresentable)Activator.CreateInstance(type, _movement, _validator, _preySpecial);
                }
                else
                {
                    animal = (IFieldPresentable)Activator.CreateInstance(type, _movement, _validator, _predatorSpecial);
                }
                bodyDictionary.Add(animal.Body, type);
                assemblyIndex++;
            }
            return bodyDictionary;
        }
    }
}
