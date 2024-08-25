using Assets.Scripts.Managers.Interfaces;
using Assets.Scripts.Services;
using Assets.Scripts.StateMachine;
using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine.Playables;

namespace Assets.Scripts.States
{
    public class LevelComplitedState : State
    {
        private ILevelManager _levelManager;
        private readonly LevelBuilder _levelBuilder;
        private readonly PlayState _playState;

        public LevelComplitedState(ISMContext context, ILevelManager levelManager, LevelBuilder levelBuilder, PlayState playState) : base(context)
        {
            _levelManager = levelManager;
            _levelBuilder = levelBuilder;
            _playState = playState;
        }

        public override async UniTask Run(CancellationToken token)
        {
            _levelManager.NextLevel();

            _levelBuilder.BuildLevel(_levelManager.CurrentLevelSequence);
            await GoTo(_playState, token);
        }
    }
}
