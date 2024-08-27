using Assets.Scripts.Services.Interfaces;
using Assets.Scripts.StateMachine;
using Cysharp.Threading.Tasks;
using System;
using Zenject;

namespace Assets.Scripts.States.Contexts
{
    public class GameSM : SMContext, IInitializable, IDisposable
    {
        private readonly LazyInject<InitState> _initState;
        private readonly ISaveLevelService _saveLevelService;

        public GameSM(LazyInject<InitState> initState, ISaveLevelService saveLevelService)
        {
            _initState = initState;
            _saveLevelService = saveLevelService;
        }

        //OnApplicationQuit
        public void Dispose()
        {
            _saveLevelService.SaveLevelStateAsync().Forget();
        }

        public void Initialize()
        {
            GoTo(_initState.Value).Forget();
        }
    }
}
