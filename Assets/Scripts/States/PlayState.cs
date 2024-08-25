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

        private UniTask? _animationTask;

        public PlayState(GridManager gridManager,IInputSystem inputSystem,
            ISMContext context, MoveBlocksManager moveBlocksManager, ILevelManager levelManager, LazyInject<LevelComplitedState> levelComplitedState) : base(context)
        {
            _gridManager = gridManager;
            _inputSystem = inputSystem;
            _moveBlocksManager = moveBlocksManager;
            _levelManager = levelManager;
            _levelComplitedState = levelComplitedState;
        }

        public override async UniTask Run(CancellationToken token)
        {
            if(_animationTask != null && _animationTask.Value.Status != UniTaskStatus.Succeeded)
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

            _animationTask = _moveBlocksManager.MoveBlockAsync(startIndex.Value, nextCell)
                .ContinueWith(() => _moveBlocksManager.Reshuffle(token));

            //await _animationTask.Value;

            if (_levelManager.IsLevelCompleted())
            {
                await GoTo(_levelComplitedState.Value, token);
            }
        }

        private Vector2Int GetStepByDirection(Direction direction)
        {
            var step = Vector2Int.zero;

            switch (direction)
            {
                case Direction.Up:
                    step.x = 1;
                    break;
                case Direction.Down:
                    step.x = -1;
                    break;
                case Direction.Right:
                    step.y = 1;
                    break;
                case Direction.Left:
                    step.y = -1;
                    break;
            }

            return step;
        }

    }
}
