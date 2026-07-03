using MzGames.Scripts.Data;
using MzGames.Scripts.Infra.Factories.Interfaces;
using MzGames.Scripts.Infra.StateMachine.States.Interfaces;
using MzGames.Scripts.Simulation;
using UnityEngine;

namespace MzGames.Scripts.Infra.StateMachine.States
{
    public class GameLoopState : IPayLoadedState<SimulationLaunch>
    {
        private readonly ISimulationFactory _simulationFactory;
        private readonly IEntityFactory _entityFactory;

        private SimulationController _simulation;

        public GameLoopState(ISimulationFactory simulationFactory, IEntityFactory entityFactory)
        {
            _simulationFactory = simulationFactory;
            _entityFactory = entityFactory;
        }

        public async void Enter(SimulationLaunch launch)
        {
            await _entityFactory.WarmUp();
            await _simulationFactory.WarmUp();

            _simulation = launch.IsResume
                ? await _simulationFactory.Restore(launch.Snapshot)
                : await _simulationFactory.Create(launch.Config);
        }

        public void Exit()
        {
            _simulation?.Dispose();
            _simulation = null;

            _simulationFactory.Cleanup();
            _entityFactory.Cleanup();
        }
    }
}
