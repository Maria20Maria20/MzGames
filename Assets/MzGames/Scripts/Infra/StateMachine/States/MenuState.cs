using System.IO;
using MzGames.Scripts.Data;
using MzGames.Scripts.Infra.Factories.Interfaces;
using MzGames.Scripts.Infra.SceneManagement;
using MzGames.Scripts.Infra.Services.PersistentData;
using MzGames.Scripts.Infra.Services.SaveLoad;
using MzGames.Scripts.Infra.StateMachine.States.Interfaces;
using MzGames.Scripts.Meta.Menu;
using UnityEngine;

namespace MzGames.Scripts.Infra.StateMachine.States
{
    public class MenuState : IState
    {
        private const string SettingsFileName = "menu-settings.json";

        private readonly GameStateMachine _gameStateMachine;
        private readonly SceneLoader _sceneLoader;
        private readonly IUIFactory _uiFactory;
        private readonly IPersistentDataService _persistentDataService;
        private readonly ISaveLoadService _saveLoadService;
        private readonly ISimulationSaveService _simulationSaveService;

        private MenuController _menu;

        public MenuState(GameStateMachine gameStateMachine, SceneLoader sceneLoader,
            IUIFactory uiFactory, IPersistentDataService persistentDataService,
            ISaveLoadService saveLoadService, ISimulationSaveService simulationSaveService)
        {
            _gameStateMachine = gameStateMachine;
            _sceneLoader = sceneLoader;
            _uiFactory = uiFactory;
            _persistentDataService = persistentDataService;
            _saveLoadService = saveLoadService;
            _simulationSaveService = simulationSaveService;
        }

        private static string SettingsPath => Path.Combine(Application.persistentDataPath, SettingsFileName);

        public async void Enter()
        {
            await _sceneLoader.Load(SceneName.Meta);

            _menu = await _uiFactory.GetOrCreateMenu();
            _menu.NewSimulationRequested += OnNewSimulation;
            _menu.ContinueRequested += OnContinue;

            _menu.Initialize(LoadSettings(), continueAvailable: _simulationSaveService.HasSave);
        }

        private SimulationConfig LoadSettings()
        {
            SimulationConfig saved = _saveLoadService.ReadFromFile<SimulationConfig>(SettingsPath);
            return saved?.Validated() ?? SimulationConfig.Default;
        }

        private void OnNewSimulation(SimulationConfig config)
        {
            config = config.Validated();
            _persistentDataService.Config = config;
            _saveLoadService.WriteToFile(SettingsPath, config);
            _gameStateMachine.Enter<LoadSimulationState, SimulationLaunch>(SimulationLaunch.New(config));
        }

        private void OnContinue()
        {
            SimulationSnapshot snapshot = _simulationSaveService.Load();
            if (snapshot == null)
                return;

            _persistentDataService.Config = snapshot.Config.Validated();
            _gameStateMachine.Enter<LoadSimulationState, SimulationLaunch>(SimulationLaunch.Resume(snapshot));
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
