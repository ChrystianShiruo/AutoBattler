using System;

namespace AutoBattle
{
    
    public enum CharacterClass : uint
    {
        Paladin = 1,
        Warrior = 2,
        Cleric = 3,
        Archer = 4
    }
    [Serializable] public class CharacterClassInfo
    {
        public Character characterChild;
        public CharacterClass Id;
        public string Name;
        public float HpModifier;
        public float ClassDamage;
        public int AttackRange;
        public CharacterSkills[] Skills;
    }

    public struct CharacterSkills
    {
        public string name;
        public string description;
        public float damage;
        public int range;
        public Status specialEffect;
    }

}
