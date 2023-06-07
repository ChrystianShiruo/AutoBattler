using System;

namespace AutoBattle
{
    public interface ICharacter
    {
        int PlayerIndex { get; }
        string Name { get; }
        float Health { get; }
        Vector2Int CurrentPosition { get; }
        ConsoleColor Color { get; }
        Action OnTurnStart { get; set; }

        void PlaceOnGrid(Grid grid, Vector2Int position);
        void HealDamage(float amount);
        bool TakeDamage(float amount);
        void Die();

        void RemoveStatus(IStatusEffect statusEffect);
        void AddStatus(IStatusEffect statusEffect);
        void StartTurn();

    }
}
