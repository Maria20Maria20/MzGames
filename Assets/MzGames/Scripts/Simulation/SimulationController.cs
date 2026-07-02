using MzGames.Scripts.Infra.Factories.Interfaces;
using MzGames.Scripts.Simulation.View;
using UnityEngine;

namespace MzGames.Scripts.Simulation
{
    public sealed class SimulationController : System.IDisposable
    {
        private readonly GameObject _root;
        private readonly SimulationView _view;
        private readonly IEntityFactory _entityFactory;
        private readonly ISimulationFactory _simulationFactory;
        private bool _disposed;

        public SimulationController(GameObject root, SimulationView view, IEntityFactory entityFactory,
            ISimulationFactory simulationFactory)
        {
            _root = root;
            _view = view;
            _entityFactory = entityFactory;
            _simulationFactory = simulationFactory;
        }

        public void Dispose()
        {
            if (_disposed)
                return;
            _disposed = true;

            _view.Dispose();
            if (_root != null)
                Object.Destroy(_root);
            _entityFactory.Cleanup();
            _simulationFactory.Cleanup();
        }
    }
}
