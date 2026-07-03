using UnityEngine;

namespace MzGames.Scripts.Simulation
{
    public sealed class Animal
    {
        public Vector2 Position;
        public Vector2 Velocity;

        public Animal(Vector2 position)
        {
            Position = position;
        }
    }
}
