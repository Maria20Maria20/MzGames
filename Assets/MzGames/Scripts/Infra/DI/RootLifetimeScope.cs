using VContainer;
using VContainer.Unity;

namespace MzGames.Scripts.Infra.DI
{
    public class RootLifetimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            new InfraInstaller().Install(builder);
        }
    }
}
