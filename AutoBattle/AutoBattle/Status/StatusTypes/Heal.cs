using System;
using System.Collections.Generic;
using System.Text;

namespace AutoBattle
{
    public class Heal : StatusEffect
    {
        public override Status _Status { get => Status.Heal; }

        public override void OnApply(Character iCharacter)
        {

            base.OnApply(Target);
            Target.HealDamage(StatusValue);
            OnRemove();
        }

        protected override void OnTick()
        {
            Target.TakeDamage(StatusValue);
            base.OnTick();
        }
    }
}
