using System;
using System.Collections.Generic;

namespace SpinalPlay
{
    public class GameStateMachine
    {
        private readonly EventBus _eventBus;
        private Dictionary<Type, IState> _states;
        private IState _currentState;
        private static bool _onceCreated = false;

        public GameStateMachine
            (DataTransferService transfer,
            UIFactory uIFactory,
            SceneLoaderService sceneLoader,
            EventBus eventBus,
            GameObjectFactory objectFactory,
            LoadingCurtain curtain,
            MusicService music)
        {
            if (!_onceCreated)
            {
                _states = new Dictionary<Type, IState>()
                {
                    [typeof(LoadMenuState)] = new LoadMenuState(uIFactory, sceneLoader, curtain, music, objectFactory, transfer),
                    [typeof(LoadPreparationSceneState)] = new LoadPreparationSceneState(uIFactory, sceneLoader, curtain, music, transfer),
                    [typeof(GameState)] = new GameState(objectFactory, sceneLoader, uIFactory, curtain, transfer),
                    [typeof(RepeatingGameState)] = new RepeatingGameState(eventBus, sceneLoader)
                };
                _onceCreated = true;
            }
            else throw new Exception("Unacceptable attempt to re-create a GameStateMachine");

            _eventBus = eventBus;

            Subscribe();
        }

        public void StartGame()
        {
            Enter<LoadMenuState>();
        }

        private void Enter<TState>() where TState : IState
        {
            if (_currentState is IStateWithExit)
            {
                ((IStateWithExit)_currentState).OnExit();
            }
            _currentState = _states[typeof(TState)];
            _currentState.OnEnter();
        }

        private void Subscribe()
        {
            _eventBus.Subscribe<EnterToPrepare>(OnPrepareEnter);
            _eventBus.Subscribe<EnterToMainMenu>(OnEnterToMenu);
            _eventBus.Subscribe<EnterToGameLoop>(OnEnterToGameLoop);
            _eventBus.Subscribe<EnterRestarting>(OnEnterRepeating);
        }

        private void OnPrepareEnter(EnterToPrepare signal)
        {
            Enter<LoadPreparationSceneState>();
        }

        private void OnEnterToMenu(EnterToMainMenu signal)
        {
            Enter<LoadMenuState>();
        }

        private void OnEnterToGameLoop(EnterToGameLoop signal)
        {
            Enter<GameState>();
        }

        private void OnEnterRepeating(EnterRestarting signal)
        {
            Enter<RepeatingGameState>();
        }
    }
}