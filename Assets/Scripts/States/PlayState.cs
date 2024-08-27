using Assets.Scripts.Managers;
using Assets.Scripts.Managers.Interfaces;
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
        private readonly MoveBlocksManager _moveBlocksManager;
        private readonly IInputSystem _inputSystem;
        private readonly LazyInject<NextLevelState> _nextLevelState;
        private readonly LazyInject<RestartLevelState> _restartLevelState;
        private readonly IUIManger _uimanger;

        private bool _isInputEnabled;

        public PlayState(IInputSystem inputSystem,
            GameSM context, MoveBlocksManager moveBlocksManager,
            ILevelManager levelManager, LazyInject<NextLevelState> nextLevelState,
            IUIManger uimanger, LazyInject<RestartLevelState> restartLevelState) : base(context)
        {
            _inputSystem = inputSystem;
            _moveBlocksManager = moveBlocksManager;
            _levelManager = levelManager;
            _nextLevelState = nextLevelState;
            _uimanger = uimanger;
            _restartLevelState = restartLevelState;
        }

        public override UniTask Enter(CancellationToken token)
        {
            _isInputEnabled = true;
            _uimanger.Next += Next;
            _uimanger.Restart += Restart;

            return base.Enter(token);
        }

        public override async UniTask Run(CancellationToken token)
        {
            if(!_isInputEnabled)
            {
                return;
            }

            var swipe = _inputSystem.CheckSwipe();

            if (swipe == null)
            {
                return;
            }

            await MoveAndReshuffle(swipe.Value.startPosition, GetStepByDirection(swipe.Value.Direction));

            if (_levelManager.IsLevelCompleted())
            {
                GoTo(_nextLevelState.Value).Forget();
            }
        }

        public override UniTask Exit()
        {
            _uimanger.Restart -= Restart;
            _uimanger.Next -= Next;
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

        private async UniTask MoveAndReshuffle(Vector3 startPosition, Vector2Int directon)
        {
            _isInputEnabled = false;
            await _moveBlocksManager.MoveBlockAsync(startPosition, directon);
            await _moveBlocksManager.CheckFallBlocksAsync();
            await _moveBlocksManager.ReshuffleAsync();
            _isInputEnabled = true;
        }

        private Vector2Int GetStepByDirection(Direction direction)
        {
            var step = Vector2Int.zero;

            switch (direction)
            {
                case Direction.Up:
                    step.y = 1;
                    break;
                case Direction.Down:
                    step.y = -1;
                    break;
                case Direction.Right:
                    step.x = 1;
                    break;
                case Direction.Left:
                    step.x = -1;
                    break;
            }

            return step;
        }

    }
}
