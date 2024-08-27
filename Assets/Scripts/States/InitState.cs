using Assets.Scripts.Data;
using Assets.Scripts.Managers.Interfaces;
using Assets.Scripts.Services;
using Assets.Scripts.Services.Interfaces;
using Assets.Scripts.StateMachine;
using Assets.Scripts.States.Contexts;
using Cysharp.Threading.Tasks;
using System.Threading;

namespace Assets.Scripts.States
{
    public class InitState : State
    {
        private readonly LevelBuilder _levelBuilder;
        private readonly ISaveLevelService _saveLevelService;
        private readonly ILevelManager _levelManager;
        private readonly PlayState _playState;

        public InitState(GameSM context, LevelBuilder levelBuilder,
            ISaveLevelService saveLevelService, ILevelManager levelManager,
            PlayState playState) : base(context)
        {
            _levelBuilder = levelBuilder;
            _saveLevelService = saveLevelService;
            _levelManager = levelManager;
            _playState = playState;
        }

        public async override UniTask Run(CancellationToken token)
        {
            await UpdateLevelStateBySavedData(token);
            _levelBuilder.BuildLevel(_levelManager.CurrentLevelSequence, _levelManager.CurrentLevelIndex);
            await GoTo(_playState, token);
        }

        private async UniTask UpdateLevelStateBySavedData(CancellationToken token)
        {
            Level savedLevel = await _saveLevelService.GetSavedDataAsync(token);
            if (savedLevel == null)
            {
                return;
            }

            _levelManager.UpdateLevel(savedLevel);
            if (_levelManager.IsLevelCompleted())
            {
                _levelManager.NextLevel();
            }
        }
    }
}
