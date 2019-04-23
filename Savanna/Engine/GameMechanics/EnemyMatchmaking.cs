using Savanna.Engine.GameMechanics.Animals.AnimalTemplates;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Savanna.Engine.GameMechanics
{
    public class EnemyMatchmaking
    {
        private Dictionary<char, char> _enemyPairings;

        public EnemyMatchmaking(Dictionary<char, Type> bodiesAndTypes)
        {
            var prey = new List<char>();
            var hunters = new List<char>();

            foreach (var item in bodiesAndTypes)
            {
                if (typeof(IHerbivore).IsAssignableFrom(item.Value))
                {
                    prey.Add(item.Key);
                }
                else if (typeof(ICarnivore).IsAssignableFrom(item.Value))
                {
                    hunters.Add(item.Key);
                }
            }
            _enemyPairings = GenerateEnemyDictionaryFromLists(prey, hunters);
        }

        private Dictionary<char, char> GenerateEnemyDictionaryFromLists(List<char> prey, List<char> hunters)
        {
            var pairings = new Dictionary<char, char>();
            if (prey.Count < hunters.Count)
            {
                for (int i = 0; i < hunters.Count; i++)
                {
                    if (i > prey.Count)
                    {
                        pairings.Add(prey[prey.Count - 1], hunters[i]);
                    }
                    else
                    {
                        pairings.Add(prey[i], hunters[i]);
                    }
                }
            }
            else
            {
                for (int i = 0; i < prey.Count; i++)
                {
                    if (i > hunters.Count)
                    {
                        pairings.Add(prey[i], hunters[hunters.Count - 1]);
                    }
                    else
                    {
                        pairings.Add(prey[i], hunters[i]);
                    }
                }
            }
            return pairings;
        }

        private Dictionary<char, char> GenerateEnemyPairings(Dictionary<char, Type> bodyAndType)
        {
            _enemyPairings = null;
            return _enemyPairings;
        }

        public char GetEnemy(char animalBody)
        {
            if (_enemyPairings.ContainsKey(animalBody))
            {
                return _enemyPairings[animalBody];
            }
            else
            {
                return _enemyPairings.FirstOrDefault(a => a.Value == animalBody).Key;
            }
        }
    }
}
