using MzGames.Scripts.Infra.StateMachine.States.Interfaces;
using UnityEngine;

namespace MzGames.Scripts.Infra.StateMachine.States
{
    public class GameLoopState: IState
    {
        public GameLoopState()
        {
        }
        public void Enter()
        {
            Application.quitting += OnExitGame;
        }

        public void Exit()
        {
            
        }

        private void OnExitGame()
        {
            Application.quitting -= OnExitGame;
        }
    }
}
