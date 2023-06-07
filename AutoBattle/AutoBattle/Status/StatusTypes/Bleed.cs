using System;
using System.Collections.Generic;
using System.Text;

namespace AutoBattle
{
    public class Bleed : StatusEffect
    {
        public override Status _Status { get => Status.Bleed; }

        public override void OnApply(Character iCharacter)
        {
            Duration = 3;// :/
            base.OnApply(Target);
        }

        protected override void OnTick()
        {
            Target.TakeDamage(StatusValue);
            base.OnTick();
        }

    }
}
