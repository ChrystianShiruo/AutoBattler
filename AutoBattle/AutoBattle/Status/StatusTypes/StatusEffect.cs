using System;
using System.Collections.Generic;
using System.Text;

namespace AutoBattle
{
    public class StatusEffect : IStatusEffect
    {
        public virtual Status _Status { get;}

        public string Name { get; set; }

        public string Description { get; set; }

        public int Duration { get; set; }

        public float StatusValue { get; set; }

        public Character Target { get; set; }

        public virtual void OnApply(Character character)
        {
            Console.WriteLine($"OnApply Called for {this}");
            //Target = character;

            Target.OnTurnStart += OnTick;
        }

        protected virtual void OnRemove()
        {
            Console.WriteLine($"OnRemove Called for {this}");
            Target.RemoveStatus(this);
            Target.OnTurnStart -= OnTick;
        }

        protected virtual void OnTick()
        {
            Console.WriteLine($"OnTick Called for {this}");

            Duration--;
            if(Duration <= 0)
            {
                OnRemove();
            }
        }
    }

    public enum Status
    {
        Bleed = 1,
        Stun = 2,
        Heal = 3
    }
}
