
using System;

namespace AutoBattle
{
    [Serializable]class Archer//TODO: Remove if not used
    {
        public CharacterClassInfo CharacterClassInfo = new CharacterClassInfo()
        {
            characterClass = CharacterClass.Archer,
            hpModifier = -20f,
            classDamage = 40f,
            attackRange = 3,
            skills = new CharacterSkills[1]
            {
                new CharacterSkills()
                {
                    name = "Bleed",
                    description = "Make target bleed for 3 turns. counter resets if applied again",
                    damage = 15f,
                    range = 3,
                    specialEffect = SpecialEffect.Bleed
                }
            }
        };
    }
}

