using Assets.Scripts.Managers;
using Assets.Scripts.Managers.Interfaces;
using Assets.Scripts.Services;
using Assets.Scripts.Services.Interfaces;
using Assets.Scripts.StateMachine;
using Assets.Scripts.States.Contexts;
using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.States
{
    public class PlayState : State
    {
        private readonly ILevelManager _levelManager;
        private readonly GridManipulatorFacade _gridManipulatorFacade;
        private readonly IInputSystem _inputSystem;
        private readonly LazyInject<NextLevelState> _nextLevelState;
        private readonly LazyInject<RestartLevelState> _restartLevelState;
        private readonly IUIManger _uiManager;

        private bool _isInputEnabled;

        public PlayState(IInputSystem inputSystem,
            GameSM context, GridManipulatorFacade gridManipulatorFacade,
            ILevelManager levelManager, LazyInject<NextLevelState> nextLevelState,
            IUIManger uimanger, LazyInject<RestartLevelState> restartLevelState) : base(context)
        {
            _inputSystem = inputSystem;
            _gridManipulatorFacade = gridManipulatorFacade;
            _levelManager = levelManager;
            _nextLevelState = nextLevelState;
            _uiManager = uimanger;
            _restartLevelState = restartLevelState;
        }

        public override UniTask Enter(CancellationToken token)
        {
            _isInputEnabled = true;
            _uiManager.Next += Next;
            _uiManager.Restart += Restart;

            return base.Enter(token);
        }

        public override async UniTask Run(CancellationToken token)
        {
            if (!_isInputEnabled)
            {
                return;
            }

            SwipeState? swipe = _inputSystem.CheckSwipe();
            if (swipe == null)
            {
                return;
            }

            Vector2Int direction = GetStepByDirection(swipe.Value.Direction);

            _isInputEnabled = false;
            await _gridManipulatorFacade.SwitchCellsThenNormalize(swipe.Value.startPosition, direction);
            _isInputEnabled = true;

            if (_levelManager.IsLevelCompleted())
            {
                GoTo(_nextLevelState.Value).Forget();
            }
        }

        public override UniTask Exit()
        {
            _uiManager.Restart -= Restart;
            _uiManager.Next -= Next;
            return base.Exit();
        }

        private void Restart()
        {
            GoTo(_restartLevelState.Value).Forget();
        }

        private void Next()
        {
            GoTo(_nextLevelState.Value).Forget();
        }

        private Vector2Int GetStepByDirection(Direction direction)
        {
            switch (direction)
            {
                case Direction.Up:
                    return Vector2Int.up;
                case Direction.Down:
                    return Vector2Int.down;
                case Direction.Right:
                    return Vector2Int.right;
                case Direction.Left:
                    return Vector2Int.left;
                default:
                    return Vector2Int.zero;
            }
        }

    }
}
