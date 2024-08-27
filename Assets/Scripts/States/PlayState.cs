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
        private readonly GridManipulatorFacade _gridManipulatorFacade;
        private readonly IInputSystem _inputSystem;
        private readonly LazyInject<NextLevelState> _nextLevelState;
        private readonly LazyInject<RestartLevelState> _restartLevelState;
        private readonly IUIManger _uimanger;

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

            var direction = GetStepByDirection(swipe.Value.Direction);

            _isInputEnabled = false;
            await _gridManipulatorFacade.SwitchCellsThenNormilize(swipe.Value.startPosition, direction);
            _isInputEnabled = true;

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
