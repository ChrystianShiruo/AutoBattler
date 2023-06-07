using System;
using System.Collections.Generic;
using System.Text;

namespace AutoBattle
{
    public class Stun : StatusEffect
    {
        public override Status _Status { get => Status.Stun; }

        public override void OnApply(Character iCharacter)
        {
            base.OnApply(Target);
            Messages.ColoredWriteLine($"{Target.Name} is stunned", Target.Color);
        }

        protected override void OnTick()
        {
            Messages.ColoredWriteLine($"{Target.Name} is stunned",Target.Color);
            base.OnTick();
        }
    }
}
