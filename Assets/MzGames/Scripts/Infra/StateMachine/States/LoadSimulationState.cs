using MzGames.Scripts.Data;
using MzGames.Scripts.Infra.Factorises.Interfaces;
using MzGames.Scripts.Infra.SceneManagement;
using MzGames.Scripts.Infra.StateMachine.States.Interfaces;

namespace MzGames.Scripts.Infra.StateMachine.States
{
    public class LoadSimulationState : IPayLoadedState<SimulationConfig>
    {
        private readonly GameStateMachine _gameStateMachine;
        private readonly SceneLoader _sceneLoader;
        private readonly IUIFactory _uiFactory;

        public LoadSimulationState(GameStateMachine gameStateMachine, SceneLoader sceneLoader,
            IUIFactory uiFactory)
        {
            _gameStateMachine = gameStateMachine;
            _sceneLoader = sceneLoader;
            _uiFactory = uiFactory;
        }

        public void BeforeEnter(SimulationConfig payLoad)
        {
        }

        public async void Enter(SimulationConfig config)
        {
            await _sceneLoader.Load(SceneName.TestTask);

            await _uiFactory.GetOrCreateUIRoot();
            await _uiFactory.GetOrCreateHUD();

            // TODO (milestone 2): build the simulation world from `config` (grid, animals, food).

            _gameStateMachine.Enter<GameLoopState>();
        }

        public void Exit()
        {
            // HUD / UI root persist into GameLoopState — nothing to tear down here.
        }
    }
}
