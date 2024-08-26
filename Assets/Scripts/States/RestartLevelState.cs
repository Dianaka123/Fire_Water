using Assets.Scripts.Managers.Interfaces;
using Assets.Scripts.Services;
using Assets.Scripts.StateMachine;
using Assets.Scripts.States.Contexts;
using Cysharp.Threading.Tasks;
using System.Threading;

namespace Assets.Scripts.States
{
    public class RestartLevelState : State
    {
        private readonly ILevelManager _levelManager;
        private readonly LevelBuilder _levelBuilder;
        private readonly PlayState _playState;

        public RestartLevelState(GameSM context, ILevelManager levelManager,
            LevelBuilder levelBuilder, PlayState playState) : base(context)
        {
            _levelManager = levelManager;
            _levelBuilder = levelBuilder;
            _playState = playState;
        }

        public override UniTask Run(CancellationToken token)
        {
            _levelManager.RestartLevel();
            _levelBuilder.RestartLevel(_levelManager.CurrentLevelSequence);
            
            GoTo(_playState).Forget();

            return UniTask.CompletedTask;
        }
    }
}
