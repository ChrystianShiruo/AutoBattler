using System;

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
    [Serializable] public class CharacterClassInfo
    {
        public CharacterClass characterClass;
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

}
