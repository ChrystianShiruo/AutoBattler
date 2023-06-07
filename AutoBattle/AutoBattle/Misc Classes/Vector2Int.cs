using System;

namespace AutoBattle
{
    public struct Vector2Int
    {
        public int x;
        public int y;
        public Vector2Int(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public static int Distance(Vector2Int vectorA, Vector2Int vectorB)
        {
            int distance = 0;
            distance += Math.Abs(vectorA.x - vectorB.x);
            distance += Math.Abs(vectorA.y - vectorB.y);
            return distance;
        }

        public static bool operator ==(Vector2Int vectorA, Vector2Int vectorB)
        {
            return (vectorA.x == vectorB.x && vectorA.y == vectorB.y);
        }

        public static bool operator !=(Vector2Int vectorA, Vector2Int vectorB)
        {
            return !(vectorA.x == vectorB.x && vectorA.y == vectorB.y);
        }

        public static Vector2Int operator -(Vector2Int vectorA, Vector2Int vectorB)
        {
            return new Vector2Int(vectorA.x - vectorB.x, vectorA.y - vectorB.y);
        }
    }

}
