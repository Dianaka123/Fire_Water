using Assets.Scripts.ScriptableObjects;
using Assets.Scripts.Data;
using Assets.Scripts.Managers.Interfaces;
using Assets.Scripts.Services.Interfaces;
using Assets.Scripts.Wrappers;
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
        public int CurrentLevelIndex => _currentLevel.LevelIndex;
        public int EmptyCellId => -1;

        private Level _currentLevel;
        private Level[] _levels;

        private CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

        public LevelManager(LevelsConfiguration levelsConfiguration, ILevelJsonConverter converter)
        {
            _configuration = levelsConfiguration;
            _converter = converter;
        }

        public void Initialize()
        {
            _levels = _converter.DeserializeAllLevels(_configuration.LevelsJSON.text);

            UpdateCurrentLevel(_levels[0]);
        }

        public void UpdateLevel(Level level)
        {
            UpdateCurrentLevel(level);
        }

        public void NextLevel()
        {
            int nextId = CurrentLevelIndex + 1;

            if (nextId >= _levels.Length)
            {
                nextId = 0;
            }

            UpdateCurrentLevel(_levels[nextId]);
        }

        public void RestartLevel()
        {
            UpdateCurrentLevel(_levels[CurrentLevelIndex]);
        }

        public bool IsLevelCompleted()
        {
            return CurrentLevelSequence.Array1D.All(x => x == EmptyCellId);
        }

        private void UpdateCurrentLevel(Level level)
        {
            _currentLevel = new Level()
            {
                LevelBlocksSequence = level.LevelBlocksSequence.Clone(),
                LevelIndex = level.LevelIndex,
            };
        }

        public void SwitchBlocks(Vector2Int from, Vector2Int to)
        {
            int block1 = CurrentLevelSequence[from];
            int block2 = CurrentLevelSequence[to];

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

        public void Dispose()
        {
            _cancellationTokenSource.Cancel();
            _cancellationTokenSource.Dispose();
        }
    }
}
