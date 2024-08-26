using Assets.Scripts.Managers;
using Assets.Scripts.Managers.Interfaces;
using Assets.Scripts.Services.Interfaces;
using Assets.Scripts.StateMachine;
using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.States
{
    public class PlayState : State
    {
        private readonly GridManager _gridManager;
        private readonly ILevelManager _levelManager;
        private readonly MoveBlocksManager _moveBlocksManager;
        private readonly IInputSystem _inputSystem;
        private readonly LazyInject<LevelComplitedState> _levelComplitedState;

        private bool _isInputEnabled;

        public PlayState(GridManager gridManager,IInputSystem inputSystem,
            ISMContext context, MoveBlocksManager moveBlocksManager, ILevelManager levelManager, LazyInject<LevelComplitedState> levelComplitedState) : base(context)
        {
            _gridManager = gridManager;
            _inputSystem = inputSystem;
            _moveBlocksManager = moveBlocksManager;
            _levelManager = levelManager;
            _levelComplitedState = levelComplitedState;
        }


        public override UniTask Enter(CancellationToken token)
        {
            _isInputEnabled = true;
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

            var swipeValue = swipe.Value;
            var startIndex = _gridManager.GetCellIndexByScreenPosition(swipeValue.startPosition);
            
            if (startIndex == null)
            {
                return;
            }

            var step = GetStepByDirection(swipeValue.Direction);

            var nextCell = startIndex.Value + step;

            await MoveAndReshuffle(startIndex.Value, nextCell);

            if (_levelManager.IsLevelCompleted())
            {
                await GoTo(_levelComplitedState.Value, token);
            }
        }

        private async UniTask MoveAndReshuffle(Vector2Int from, Vector2Int to)
        {
            _isInputEnabled = false;
            await _moveBlocksManager.MoveBlockAsync(from, to);
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
