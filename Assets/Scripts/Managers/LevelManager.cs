using Assets.Scripts.Configs;
using Assets.Scripts.Managers.Interfaces;
using Assets.Scripts.Services;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.Managers
{
    public class LevelManager : ILevelManager, IInitializable, IDisposable
    {
        private readonly LevelsConfiguration _configuration;
        private readonly LevelJsonConverter _converter;

        public int[,] CurrentLevelSequence => _currentLevel.LevelBlocksSequence;
        public int EmptyCellId => -1;

        private Level _currentLevel;
        private Level[] _levels;
        private int _levelId = 0;
        
        private CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

        public LevelManager(LevelsConfiguration levelsConfiguration, LevelJsonConverter converter)
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
            foreach(var it in CurrentLevelSequence)
            {
                if(it != EmptyCellId)
                {
                    return false;
                }
            }

            return true;
        } 

        public void Dispose()
        {
            _cancellationTokenSource.Cancel();
            _cancellationTokenSource.Dispose();
        }

        private void UpdateCurrentLevel(Level level)
        {
            _currentLevel = level;
        }

        public void SwitchBlocks(Vector2Int from, Vector2Int to)
        {
            var block1 = CurrentLevelSequence[from.x, from.y];
            var block2 = CurrentLevelSequence[to.x, to.y];

            CurrentLevelSequence[from.x, from.y] = block2;
            CurrentLevelSequence[to.x, to.y] = block1;
        }

        public void SetEmptyCell(Vector2Int cellIndex)
        {
            CurrentLevelSequence[cellIndex.x, cellIndex.y] = EmptyCellId;
        }

        public bool IsEmptyCell(Vector2Int cellIndex)
        {
            return CurrentLevelSequence[cellIndex.x, cellIndex.y] == EmptyCellId;
        }
    }
}
