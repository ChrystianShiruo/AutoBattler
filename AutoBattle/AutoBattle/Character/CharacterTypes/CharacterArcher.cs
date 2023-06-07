
using System;

namespace AutoBattle
{
    public class CharacterArcher : Character
    {
        // TODO: Implement special rules
        public CharacterArcher(CharacterClassInfo characterClassInfo, int id, ColorScheme color, int teamId) : base (characterClassInfo, id,color, teamId)
        {
            
        }

        public override void StartTurn()
        {
            base.StartTurn();
            //Do different behavior, like move away from target if close and with allies left, or only move to line up a shot
        }


    }  
}
