using System;
using UnityEngine;

namespace MzGames.Scripts.Data
{
    [Serializable]
    public class SimulationConfig
    {
        public const int MinGrid = 2;
        public const int MaxGrid = 1000;
        public const float MinSpeed = 1f;
        public const float MaxSpeed = 100f;

        public int GridSize; // N
        public int Count;    // M (animals == food)
        public float Speed;  // V

        public static int MaxCount(int gridSize) => gridSize * gridSize / 2;

        public static SimulationConfig Default => new SimulationConfig
        {
            GridSize = 16,
            Count = 16,
            Speed = 5f
        }.Validated();

        public SimulationConfig Validated()
        {
            GridSize = Mathf.Clamp(GridSize, MinGrid, MaxGrid);
            Speed = Mathf.Clamp(Speed, MinSpeed, MaxSpeed);
            Count = Mathf.Clamp(Count, 0, MaxCount(GridSize));
            return this;
        }
    }
}
