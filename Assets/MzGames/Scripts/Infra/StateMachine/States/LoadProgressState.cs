using MzGames.Scripts.Infra.Factories.Interfaces;
using MzGames.Scripts.Infra.StateMachine.States.Interfaces;
using UnityEngine;

namespace MzGames.Scripts.Infra.StateMachine.States
{
    public class LoadProgressState : IState
    {
        private readonly GameStateMachine _gameStateMachine;
        private readonly IUIFactory _uiFactory;

        public LoadProgressState(GameStateMachine gameStateMachine, IUIFactory uiFactory)
        {
            _gameStateMachine = gameStateMachine;
            _uiFactory = uiFactory;
        }

        public async void Enter()
        {
            await _uiFactory.WarmUp();

            _gameStateMachine.Enter<MenuState>();
        }

        public void Exit()
        {
        }
    }
}
