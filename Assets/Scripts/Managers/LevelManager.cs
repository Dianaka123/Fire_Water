using Assets.Scripts.Configs;
using Assets.Scripts.Managers.Interfaces;
using Assets.Scripts.Services.Interfaces;
using Assets.Scripts.Wrappers;
using Moq;
using System;
using System.Linq;
using System.Threading;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.Managers
{
    public class LevelManager : ILevelManager, IInitializable, IDisposable
    {
        private readonly LevelsConfiguration _configuration;
        private readonly ILevelJsonConverter _converter;

        public Array2D<int> CurrentLevelSequence => _currentLevel.LevelBlocksSequence;
        public int EmptyCellId => -1;

        private Level _currentLevel;
        private Level[] _levels;
        private int _levelId = 0;
        
        private CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

        public LevelManager(LevelsConfiguration levelsConfiguration, ILevelJsonConverter converter)
        {
            _configuration = levelsConfiguration;
            _converter = converter;
        }

        public void Initialize()
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

        public bool IsLevelCompleted()
        {
            return CurrentLevelSequence.Array1D.All(x => x == EmptyCellId);
        } 

        public void Dispose()
        {
            _cancellationTokenSource.Cancel();
            _cancellationTokenSource.Dispose();
        }

        private void UpdateCurrentLevel(Level level)
        {
            _currentLevel = new Level()
            {
                LevelBlocksSequence = level.LevelBlocksSequence.Clone(),
            };
        }

        public void SwitchBlocks(Vector2Int from, Vector2Int to)
        {
            var block1 = CurrentLevelSequence[from];
            var block2 = CurrentLevelSequence[to];

            CurrentLevelSequence[from] = block2;
            CurrentLevelSequence[to] = block1;
        }

        public void SetEmptyCell(Vector2Int cellIndex)
        {
            CurrentLevelSequence[cellIndex] = EmptyCellId;
        }

        public bool IsEmptyCell(Vector2Int cellIndex)
        {
            return CurrentLevelSequence[cellIndex] == EmptyCellId;
        }
    }
}
