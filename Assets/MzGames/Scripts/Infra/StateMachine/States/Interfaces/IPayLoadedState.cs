namespace MzGames.Scripts.Infra.StateMachine.States.Interfaces
{
    public interface IPayLoadedState<TPayLoad> : IExitableState
    {
        void Enter(TPayLoad payLoad);
    }
}
