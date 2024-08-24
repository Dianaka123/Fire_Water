using Assets.Scripts.Managers.Interfaces;
using Assets.Scripts.Services;
using Assets.Scripts.Services.Interfaces;
using Assets.Scripts.StateMachine;
using Cysharp.Threading.Tasks;
using System.Threading;

namespace Assets.Scripts.States
{
    public class InitState : State
    {
        private readonly LevelBuilder _levelBuilder;
        private readonly ISaveLevelService _saveLevelService;
        private readonly ILevelManager _levelManager;
        private readonly EmptyState _emptyState;

        public InitState(ISMContext context, LevelBuilder levelBuilder, ISaveLevelService saveLevelService, ILevelManager levelManager, EmptyState emptyState) : base(context)
        {
            _levelBuilder = levelBuilder;
            _saveLevelService = saveLevelService;
            _levelManager = levelManager;
            _emptyState = emptyState;
        }

        public async override UniTask Run(CancellationToken token)
        {
            await UpdateLevelStateBySavingData(token);
            var level = _levelManager.CurrentLevel;

            _levelBuilder.BuildLevel(level);
            GoTo(_emptyState, token).Forget();
        }

        private async UniTask UpdateLevelStateBySavingData(CancellationToken token)
        {
            var saving = await _saveLevelService.GetSavedDataAsync(token);

            if(saving == null)
            {
                return;
            }

            _levelManager.UpdateLevelBySavingData(saving);
        }
    }
}
