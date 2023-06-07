namespace AutoBattle
{    public interface IStatusEffect
    {
        Status _Status { get;  }
        string Name { get; }
        string Description { get; }
        int Duration { get; }
        public float StatusValue { get; }

        public Character Target { get; }

        void OnApply(Character iCharacter);
        //void OnTick(ICharacter iCharacter);
        //void OnRemove(ICharacter iCharacter);

    }
}