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
        private readonly LevelManager _levelManager;
        private readonly IInputSystem _inputSystem;

        public PlayState(GridManager gridManager,IInputSystem inputSystem,
            ISMContext context, LevelManager levelManager) : base(context)
        {
            _gridManager = gridManager;
            _inputSystem = inputSystem;
            _levelManager = levelManager;
        }

        public override UniTask Run(CancellationToken token)
        {
            var direction = _inputSystem.Direction;
            var touch = _inputSystem.GetInputTouch();

            if (touch.HasValue && direction != Direction.None && _inputSystem.IsSwiping)
            {
                var cellIndex = _gridManager.GetCellIndexByScreenPosition(touch.Value.position);
                if(Vector2.one * -1 != cellIndex)
                {

                }
            }
            return UniTask.CompletedTask;
        }
    }
}
