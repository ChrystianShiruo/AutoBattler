using System;
using System.Collections.Generic;
using System.Text;

namespace AutoBattle
{
    public enum SpecialEffect
    {
        Bleed = 1,
        Stun = 2,
        Heal = 3
    }
    public enum CharacterClass : uint
    {
        Paladin = 1,
        Warrior = 2,
        Cleric = 3,
        Archer = 4
    }
    [Serializable]public class CharacterClassInfo
    {
        public CharacterClass characterClass;
        //Character CharacterClass;
        public float hpModifier;
        public float classDamage;
        public int attackRange;
        public CharacterSkills[] skills;

    }

    public struct CharacterSkills
    {
        public string name;
        public string description;
        public float damage;
        public int range;
        public SpecialEffect specialEffect;
    }
    
    //public class CharacterClassUtils
    //{
    //    public static KeyValuePair<string, uint>[] GetCharacterClasses()
    //    {
    //        List<KeyValuePair<string, uint>> classList = new List<KeyValuePair<string, uint>>();

    //        string[] characterClassNames = Enum.GetNames(typeof(CharacterClass));
    //        foreach(string name in characterClassNames)
    //        {
    //            classList.Add(new KeyValuePair<string, uint>(name, (uint)Enum.Parse(typeof(CharacterClass), name)));
    //        }

    //        return classList.ToArray();
    //    }
    //}
}
