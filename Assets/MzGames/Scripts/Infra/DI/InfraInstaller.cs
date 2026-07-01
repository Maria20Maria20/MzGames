using MzGames.Scripts.Infra.AssetManagement;
using MzGames.Scripts.Infra.Factorises;
using MzGames.Scripts.Infra.Factorises.Interfaces;
using MzGames.Scripts.Infra.SceneManagement;
using MzGames.Scripts.Infra.Services.PersistentData;
using MzGames.Scripts.Infra.Services.SaveLoad;
using MzGames.Scripts.Infra.StateMachine;
using MzGames.Scripts.Infra.StateMachine.States;
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
            builder.Register<ISaveLoadService, LocalSaveLoadService>(Lifetime.Singleton);

            builder.Register<AssetProvider>(Lifetime.Singleton)
                .As<IAssetProvider>()
                .As<IInitializable>();

            builder.Register<IUIFactory, UIFactory>(Lifetime.Singleton);

            builder.Register<SceneLoader>(Lifetime.Singleton);
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
