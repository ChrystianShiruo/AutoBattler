using System;
using System.Collections.Generic;
using System.Text;

namespace AutoBattle
{
    public class Types
    {
        public enum CharacterClass : uint
        {
            Paladin = 1,
            Warrior = 2,
            Cleric = 3,
            Archer = 4,
            Assassin = 6
        }
        public static KeyValuePair<string, uint>[] GetCharacterClasses()
        {
            List<KeyValuePair<string, uint>> classList = new List<KeyValuePair<string, uint>>();

            string[] characterClassNames = Enum.GetNames(typeof(CharacterClass));
            foreach(string name in characterClassNames)
            {
                classList.Add(new KeyValuePair<string, uint>(name, (uint)Enum.Parse(typeof(CharacterClass), name)));
            }

            return classList.ToArray();
        }

        public struct CharacterClassSpecific
        {
            CharacterClass CharacterClass;
            float hpModifier;
            float ClassDamage;
            CharacterSkills[] skills;

        }        
        
        public struct CharacterSkills
        {
            string Name;
            float damage;
            float damageMultiplier;
        }

        public struct Vector2Int
        {
            public int x;
            public int y;
            public Vector2Int(int x, int y)
            {
                this.x = x;
                this.y = y;
            }

            public static int Distance(Vector2Int vectorA, Vector2Int vectorB)
            {
                int distance = 0;

                distance += Math.Abs(vectorA.x - vectorB.x);
                distance += Math.Abs(vectorA.y - vectorB.y);

                return distance;
            }

            /// <summary>
            /// Moves on the X and Y axis until it reaches target or moves coveredDistance. Does not move diagonally.
            /// </summary>
            /// <param name="start"></param>
            /// <param name="target"></param>
            /// <param name="coveredDistance"></param>
            /// <returns></returns>
            public static Vector2Int MoveTowards(Vector2Int start, Vector2Int target, int coveredDistance)// TODO: remove if unused
            {
                //if(start == target) Faster to not have this most of the time, lets trust method caller :), we return before iterating loop anyway
                //{
                //    return start;
                //}

                Vector2Int relativePosition = target - start;
                Vector2Int newPosition = start;
                for(int i = 0; i < coveredDistance; i++)
                {
                    if(newPosition == target)
                    {
                        break;
                    }

                    if(newPosition.x != target.x && true)
                    {
                        newPosition = new Vector2Int(Math.Sign(target.x - newPosition.x), newPosition.y);
                    } else
                    {
                        newPosition = new Vector2Int(newPosition.x, Math.Sign(target.y - newPosition.y));
                    }
                }

                return newPosition;
            }

            public static bool operator ==(Vector2Int vectorA, Vector2Int vectorB)
            {
                return (vectorA.x == vectorB.x && vectorA.y == vectorB.y);
            }

            public static bool operator !=(Vector2Int vectorA, Vector2Int vectorB)
            {
                return !(vectorA.x == vectorB.x && vectorA.y == vectorB.y);
            }

            public static Vector2Int operator -(Vector2Int vectorA, Vector2Int vectorB)
            {
                return new Vector2Int(vectorA.x - vectorB.x, vectorA.y - vectorB.y);
            }

        }

        public class GridCell
        {
            public Character occupied;
            public int Index;
            public Vector2Int position;
            public GridCell(int x, int y, Character occupied, int index)
            {
                this.occupied = occupied;
                this.Index = index;
                position = new Vector2Int(x, y);
            }

        }
        
    }
}
