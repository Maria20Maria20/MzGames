using System;
using System.Collections.Generic;
using MzGames.Scripts.Infra.Factories;
using MzGames.Scripts.Infra.StateMachine.States;
using MzGames.Scripts.Infra.StateMachine.States.Interfaces;
using VContainer.Unity;

namespace MzGames.Scripts.Infra.StateMachine
{
    public class GameStateMachine: IInitializable
    {
        private readonly StateFactory _stateFactory; 

        private Dictionary<Type, IExitableState> _states;
        private IExitableState _currentState;

        public GameStateMachine(StateFactory stateFactory)
        {
            _stateFactory = stateFactory;
        }
        public void Initialize()
        {
            _states = new Dictionary<Type, IExitableState>()
            {
                [typeof(BootstrapState)] = _stateFactory.CreateState<BootstrapState>(),
                [typeof(LoadProgressState)] = _stateFactory.CreateState<LoadProgressState>(),
                [typeof(MenuState)] = _stateFactory.CreateState<MenuState>(),
                [typeof(LoadSimulationState)] = _stateFactory.CreateState<LoadSimulationState>(),
                [typeof(GameLoopState)] = _stateFactory.CreateState<GameLoopState>(),
            };
            Enter<BootstrapState>();
        }

        public void Enter<TState>() where TState : class, IState => ChangeState<TState>().Enter();

        public void Enter<TState, TPayLoad>(TPayLoad payLoad) where TState : class, IPayLoadedState<TPayLoad> =>
            ChangeState<TState>().Enter(payLoad);
        private TState ChangeState<TState>() where TState : class, IExitableState
        {
            _currentState?.Exit();
            var state = GetState<TState>();
            _currentState = state;
            UnityEngine.Debug.Log($"state changed to {_currentState.GetType().Name}");
            return state;
        }

        private TState GetState<TState>() where TState : class, IExitableState => _states[typeof(TState)] as TState;

    }
}
