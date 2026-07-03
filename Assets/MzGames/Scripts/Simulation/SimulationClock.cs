using MzGames.Scripts.Simulation.Interfaces;
using UnityEngine;

namespace MzGames.Scripts.Simulation
{
    public class SimulationClock : ISimulationClock
    {
        public const float MinSpeed = 0f;
        public const float MaxSpeed = 1000f;
        public const float DefaultSpeed = 1f;

        private float _speed = DefaultSpeed;

        public float Speed
        {
            get => _speed;
            set => _speed = Mathf.Clamp(value, MinSpeed, MaxSpeed);
        }
    }
}
