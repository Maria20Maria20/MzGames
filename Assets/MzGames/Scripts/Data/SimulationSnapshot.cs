using System;

namespace MzGames.Scripts.Data
{
    [Serializable]
    public class SimulationSnapshot
    {
        public SimulationConfig Config;
        public AnimalSnapshot[] Animals;
        public FoodSnapshot[] Foods;

        public bool IsValid =>
            Config != null &&
            Animals != null &&
            Foods != null &&
            Animals.Length == Foods.Length &&
            Animals.Length == Config.Count;
    }

    [Serializable]
    public class AnimalSnapshot
    {
        public float X;
        public float Z;
        public float VX;
        public float VZ;
    }

    [Serializable]
    public class FoodSnapshot
    {
        public int Cell;
        public float X;
        public float Z;
    }
}
