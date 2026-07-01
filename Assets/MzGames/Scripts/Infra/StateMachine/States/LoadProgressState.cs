using MzGames.Scripts.Infra.StateMachine.States.Interfaces;

namespace MzGames.Scripts.Infra.StateMachine.States
{
    public class LoadProgressState : IState
    {
        private readonly GameStateMachine _gameStateMachine;

        public LoadProgressState(GameStateMachine gameStateMachine)
        {
            _gameStateMachine = gameStateMachine;
        }

        public void Enter()
        {
            _gameStateMachine.Enter<MenuState>();
        }

        public void Exit()
        {
        }
    }
}
