using Assets.Scripts.Managers.Interfaces;
using Assets.Scripts.Services;
using Assets.Scripts.StateMachine;
using Assets.Scripts.States.Contexts;
using Cysharp.Threading.Tasks;
using System.Threading;

namespace Assets.Scripts.States
{
    public class NextLevelState : State
    {
        private ILevelManager _levelManager;
        private readonly LevelBuilder _levelBuilder;
        private readonly PlayState _playState;

        public NextLevelState(GameSM context, ILevelManager levelManager, LevelBuilder levelBuilder, PlayState playState) : base(context)
        {
            _levelManager = levelManager;
            _levelBuilder = levelBuilder;
            _playState = playState;
        }

        public override async UniTask Run(CancellationToken token)
        {
            _levelManager.NextLevel();
            _levelBuilder.BuildLevel(_levelManager.CurrentLevelSequence, _levelManager.CurrentLevelIndex);
            await GoTo(_playState, token);
        }
    }
}
