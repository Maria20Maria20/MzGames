using MzGames.Scripts.Simulation.View;
using UnityEngine;

namespace MzGames.Scripts.Simulation
{
    public class SimulationController : System.IDisposable
    {
        private readonly GameObject _root;
        private readonly SimulationView _view;
        private bool _disposed;

        public SimulationController(GameObject root, SimulationView view)
        {
            _root = root;
            _view = view;
        }

        public void Dispose()
        {
            if (_disposed)
                return;
            _disposed = true;

            _view.Dispose();
            if (_root != null)
                Object.Destroy(_root);
        }
    }
}
