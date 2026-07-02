using UnityEngine;

namespace MzGames.Scripts.Simulation
{
    public sealed class Animal
    {
        public readonly int PairId;
        public Vector2 Position;
        public Vector2 Velocity;

        public Animal(int pairId, Vector2 position)
        {
            PairId = pairId;
            Position = position;
        }
    }
}
