using MzGames.Scripts.Data;
using MzGames.Scripts.Infra.Factories.Interfaces;
using MzGames.Scripts.Infra.StateMachine.States.Interfaces;
using MzGames.Scripts.Simulation;
using UnityEngine;

namespace MzGames.Scripts.Infra.StateMachine.States
{
    public class GameLoopState : IPayLoadedState<SimulationConfig>
    {
        private readonly ISimulationFactory _simulationFactory;
        private readonly IEntityFactory _entityFactory;

        private SimulationController _simulation;

        public GameLoopState(ISimulationFactory simulationFactory, IEntityFactory entityFactory)
        {
            _simulationFactory = simulationFactory;
            _entityFactory = entityFactory;
        }

        public void BeforeEnter(SimulationConfig payLoad)
        {
        }

        public async void Enter(SimulationConfig config)
        {
            await _entityFactory.WarmUp();
            await _simulationFactory.WarmUp();

            _simulation = await _simulationFactory.Create(config);
        }

        public void Exit()
        {
            _simulation?.Dispose();
            _simulation = null;
        }
    }
}
