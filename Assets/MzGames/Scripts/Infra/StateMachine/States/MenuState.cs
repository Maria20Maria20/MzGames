using MzGames.Scripts.Data;
using MzGames.Scripts.Infra.Factories.Interfaces;
using MzGames.Scripts.Infra.SceneManagement;
using MzGames.Scripts.Infra.Services.PersistentData;
using MzGames.Scripts.Infra.StateMachine.States.Interfaces;
using MzGames.Scripts.Meta.Menu;

namespace MzGames.Scripts.Infra.StateMachine.States
{
    public class MenuState : IState
    {
        private readonly GameStateMachine _gameStateMachine;
        private readonly SceneLoader _sceneLoader;
        private readonly IUIFactory _uiFactory;
        private readonly IPersistentDataService _persistentDataService;

        private MenuController _menu;

        public MenuState(GameStateMachine gameStateMachine, SceneLoader sceneLoader,
            IUIFactory uiFactory, IPersistentDataService persistentDataService)
        {
            _gameStateMachine = gameStateMachine;
            _sceneLoader = sceneLoader;
            _uiFactory = uiFactory;
            _persistentDataService = persistentDataService;
        }

        public async void Enter()
        {
            await _sceneLoader.Load(SceneName.Meta);

            _menu = await _uiFactory.GetOrCreateMenu();
            _menu.NewSimulationRequested += OnNewSimulation;
            _menu.ContinueRequested += OnContinue;

            _menu.Initialize(SimulationConfig.Default, continueAvailable: false);
        }

        private void OnNewSimulation(SimulationConfig config)
        {
            _persistentDataService.Config = config.Validated();
            _gameStateMachine.Enter<LoadSimulationState, SimulationConfig>(_persistentDataService.Config);
        }

        private void OnContinue()
        {
            // TODO (save milestone): load the saved SimulationState and start LoadSimulationState.
        }

        public void Exit()
        {
            if (_menu != null)
            {
                _menu.NewSimulationRequested -= OnNewSimulation;
                _menu.ContinueRequested -= OnContinue;
                _menu = null;
            }

            _uiFactory.DestroyMenu();
        }
    }
}
