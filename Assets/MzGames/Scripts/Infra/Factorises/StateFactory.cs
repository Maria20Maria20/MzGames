using MzGames.Scripts.Infra.StateMachine.States.Interfaces;
using VContainer;

namespace MzGames.Scripts.Infra.Factorises
{
    public class StateFactory
    {
        private readonly IObjectResolver _container;

        public StateFactory(IObjectResolver container) => _container = container;
        public T CreateState<T>() where T : IExitableState => _container.Resolve<T>();
    }
}
