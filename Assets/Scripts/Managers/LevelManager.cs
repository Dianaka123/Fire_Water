using Assets.Scripts.Configs;
using Assets.Scripts.Managers.Interfaces;
using Assets.Scripts.Services;
using System;
using System.Threading;
using Zenject;

namespace Assets.Scripts.Managers
{
    public class LevelManager : ILevelManager, IInitializable, IDisposable
    {
        private readonly LevelsConfiguration _configuration;
        private readonly LevelJsonConverter _converter;
        public Level CurrentLevel { get; private set; }

        private Level[] _levels;
        private int _levelId = 0;
        private CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

        public LevelManager(LevelsConfiguration levelsConfiguration, LevelJsonConverter converter)
        {
            _configuration = levelsConfiguration;
            _converter = converter;
        }

        public async void Initialize()
        {
            _levels = _converter.DeserializeAllLevels(_configuration.LevelsJSON.text);
            UpdateCurrentLevel(_levels[_levelId]);
        }

        public void UpdateLevelBySavingData(Level level)
        {
            UpdateCurrentLevel(level);
        }

        public void NextLevel()
        {
            _levelId++;

            if(_levelId >= _levels.Length)
            {
                _levelId = 0;
            }

            UpdateCurrentLevel(_levels[_levelId]);
        }

        public void Dispose()
        {
            _cancellationTokenSource.Cancel();
            _cancellationTokenSource.Dispose();
        }

        private void UpdateCurrentLevel(Level level)
        {
            CurrentLevel = level;
        }
    }
}
