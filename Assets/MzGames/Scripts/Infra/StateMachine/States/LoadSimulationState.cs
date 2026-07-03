using MzGames.Scripts.Data;
using MzGames.Scripts.Infra.Factories.Interfaces;
using MzGames.Scripts.Infra.SceneManagement;
using MzGames.Scripts.Infra.StateMachine.States.Interfaces;

namespace MzGames.Scripts.Infra.StateMachine.States
{
    public class LoadSimulationState : IPayLoadedState<SimulationLaunch>
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

        public async void Enter(SimulationLaunch launch)
        {
            await _sceneLoader.Load(SceneName.TestTask);

            await _uiFactory.GetOrCreateUIRoot();
            await _uiFactory.GetOrCreateHUD();

            _gameStateMachine.Enter<GameLoopState, SimulationLaunch>(launch);
        }

        public void Exit()
        {
        }
    }
}
