using MzGames.Scripts.Infra.AssetManagement;
using MzGames.Scripts.Infra.Factories;
using MzGames.Scripts.Infra.Factories.Interfaces;
using MzGames.Scripts.Infra.SceneManagement;
using MzGames.Scripts.Infra.Services.PersistentData;
using MzGames.Scripts.Infra.Services.SaveLoad;
using MzGames.Scripts.Infra.StateMachine;
using MzGames.Scripts.Infra.StateMachine.States;
using MzGames.Scripts.Simulation;
using MzGames.Scripts.Simulation.Interfaces;
using VContainer;
using VContainer.Unity;

namespace MzGames.Scripts.Infra.DI
{
    public class InfraInstaller : IInstaller
    {
        public void Install(IContainerBuilder builder)
        {
            RegisterServices(builder);
            RegisterStateMachine(builder);
        }

        private void RegisterServices(IContainerBuilder builder)
        {
            builder.Register<IPersistentDataService, PersistentDataService>(Lifetime.Singleton);
            builder.Register<ISaveLoadService, SaveLoadService>(Lifetime.Singleton);
            builder.Register<ISimulationSaveService, SimulationSaveService>(Lifetime.Singleton);

            builder.Register<AssetProvider>(Lifetime.Singleton)
                .As<IAssetProvider>()
                .As<IInitializable>();

            builder.Register<IUIFactory, UIFactory>(Lifetime.Singleton);

            builder.Register<SceneLoader>(Lifetime.Singleton);

            builder.Register<ISimulationClock, SimulationClock>(Lifetime.Singleton);
            builder.Register<IEntityFactory, EntityFactory>(Lifetime.Singleton);
            builder.Register<ISimulationFactory, SimulationFactory>(Lifetime.Singleton);
        }

        private void RegisterStateMachine(IContainerBuilder builder)
        {
            builder.Register<StateFactory>(Lifetime.Singleton);

            builder.Register<BootstrapState>(Lifetime.Singleton);
            builder.Register<LoadProgressState>(Lifetime.Singleton);
            builder.Register<MenuState>(Lifetime.Singleton);
            builder.Register<LoadSimulationState>(Lifetime.Singleton);
            builder.Register<GameLoopState>(Lifetime.Singleton);

            builder.RegisterEntryPoint<GameStateMachine>().AsSelf();
        }
    }
}
