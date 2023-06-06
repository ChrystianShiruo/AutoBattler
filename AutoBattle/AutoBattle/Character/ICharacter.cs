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

        void PlaceOnGrid(Grid grid, Vector2Int position);
        bool TakeDamage(float amount);
        void Die();
        void StartTurn();

    }
}
