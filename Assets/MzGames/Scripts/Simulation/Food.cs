using UnityEngine;

namespace MzGames.Scripts.Simulation
{
    public class Food
    {
        public int Cell;
        public Vector2 Position;

        public Food(int cell, Vector2 position)
        {
            Cell = cell;
            Position = position;
        }
    }
}
