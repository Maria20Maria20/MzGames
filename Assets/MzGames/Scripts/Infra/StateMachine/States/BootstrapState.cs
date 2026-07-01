using MzGames.Scripts.Infra.StateMachine.States.Interfaces;

namespace MzGames.Scripts.Infra.StateMachine.States
{
    public class BootstrapState: IState
    {
        private readonly GameStateMachine _gameStateMachine;

        public BootstrapState(GameStateMachine stateMachine)
        {
            _gameStateMachine = stateMachine;
        }

        public void Enter()
        {
            _gameStateMachine.Enter<LoadProgressState>();
        }

        public void Exit()
        {
            
        }
    }
}
