using UnityEngine;

namespace MzGames.Scripts.Simulation
{
    public sealed class Food
    {
        public readonly int PairId;
        public int Cell;
        public Vector2 Position;

        public Food(int pairId, int cell, Vector2 position)
        {
            PairId = pairId;
            Cell = cell;
            Position = position;
        }
    }
}
