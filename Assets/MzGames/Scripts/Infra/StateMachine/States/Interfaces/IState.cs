namespace MzGames.Scripts.Infra.StateMachine.States.Interfaces
{
    public interface IState: IExitableState
    {
        public void Enter();
    }
}
