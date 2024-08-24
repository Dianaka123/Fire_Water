using Assets.Scripts.Managers;
using Assets.Scripts.Services.Interfaces;
using Assets.Scripts.StateMachine;
using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;

namespace Assets.Scripts.States
{
    public class PlayState : State
    {
        private readonly GridManager _gridManager;
        private readonly MoveBlocksManager _moveBlocksManager;
        private readonly IInputSystem _inputSystem;

        public PlayState(GridManager gridManager,IInputSystem inputSystem,
            ISMContext context, MoveBlocksManager moveBlocksManager) : base(context)
        {
            _gridManager = gridManager;
            _inputSystem = inputSystem;
            _moveBlocksManager = moveBlocksManager;
        }

        public override UniTask Run(CancellationToken token)
        {
            var swipe = _inputSystem.CheckSwipe();

            if (swipe != null)
            {
                var swipeValue = swipe.Value;
                var startIndex = _gridManager.GetCellIndexByScreenPosition(swipeValue.startPosition);
                if(Vector2.one * -1 != startIndex)
                {
                    var step = GetStepByDirection(swipeValue.Direction);
                    
                    var nextCell = startIndex + step;
                    _moveBlocksManager.MoveBlock(startIndex, nextCell);
                }
            }
            return UniTask.CompletedTask;
        }

        private Vector2Int GetStepByDirection(Direction direction)
        {
            var step = Vector2Int.zero;

            switch (direction)
            {
                case Direction.Up:
                    step.x = -1;
                    break;
                case Direction.Down:
                    step.x = 1;
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
