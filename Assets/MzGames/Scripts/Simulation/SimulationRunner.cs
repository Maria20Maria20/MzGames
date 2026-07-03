using MzGames.Scripts.Simulation.Interfaces;
using MzGames.Scripts.Simulation.View;
using UnityEngine;

namespace MzGames.Scripts.Simulation
{
    public sealed class SimulationRunner : MonoBehaviour
    {
        private const float MaxUnitsPerStep = SimulationWorld.EatRadius; 
        private const int MaxSubSteps = 16;         

        private SimulationWorld _world;
        private SimulationView _view;
        private ISimulationClock _clock;
        private float _speed; 

        public void Initialize(SimulationWorld world, SimulationView view, ISimulationClock clock, float speed)
        {
            _world = world;
            _view = view;
            _clock = clock;
            _speed = Mathf.Max(0.0001f, speed);
        }

        private void Update()
        {
            if (_world == null)
                return;

            float scaled = Time.deltaTime * _clock.Speed;
            if (scaled > 0f)
            {
                float maxStep = MaxUnitsPerStep / _speed; 
                int steps = Mathf.Clamp(Mathf.CeilToInt(scaled / maxStep), 1, MaxSubSteps);
                float dt = scaled / steps;

                for (int s = 0; s < steps; s++)
                    _world.Tick(dt);
            }

            _view.SyncTransforms();
        }
    }
}
