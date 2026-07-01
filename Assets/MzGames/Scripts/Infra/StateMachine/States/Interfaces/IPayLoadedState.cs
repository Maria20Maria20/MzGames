namespace MzGames.Scripts.Infra.StateMachine.States.Interfaces
{
    public interface IPayLoadedState<TPayLoad> : IExitableState
    {
        void BeforeEnter(TPayLoad payLoad);
        void Enter(TPayLoad payLoad);
    }
}
