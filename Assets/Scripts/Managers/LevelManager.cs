using Assets.Scripts.Configs;
using Assets.Scripts.Managers.Interfaces;
using Assets.Scripts.Services.Data;
using System;
using System.Threading;
using Unity.Plastic.Newtonsoft.Json;
using Zenject;

namespace Assets.Scripts.Managers
{
    public class LevelManager : ILevelManager, IInitializable, IDisposable
    {
        private readonly LevelsConfiguration _configuration;
        public LevelDesc CurrentLevel { get; private set; }

        private Levels _levels;
        private int _levelId = 0;
        private CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

        public LevelManager(LevelsConfiguration levelsConfiguration)
        {
            _configuration = levelsConfiguration;
        }

        public async void Initialize()
        {
            _levels = JsonConvert.DeserializeObject<Levels>(_configuration.LevelsJSON.text);
            UpdateCurrentLevel(_levels.LevelsDesc[_levelId]);
        }

        public void UpdateLevelBySavingData(LevelSavingData levelSavingData)
        {
            UpdateCurrentLevel(levelSavingData.LevelDesc);
        }

        public LevelDesc GetLevelDescById(int levelId)
        {
            return _levels.LevelsDesc[levelId];
        }

        public void NextLevel()
        {
            _levelId++;

            if(_levelId >= _levels.LevelsDesc.Length)
            {
                _levelId = 0;
            }

            UpdateCurrentLevel(_levels.LevelsDesc[_levelId]);
        }

        public void Dispose()
        {
            _cancellationTokenSource.Cancel();
            _cancellationTokenSource.Dispose();
        }

        private void UpdateCurrentLevel(LevelDesc levelDesc)
        {
            CurrentLevel = levelDesc;
        }
    }
}
