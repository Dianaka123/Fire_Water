using Assets.Scripts.ScriptableObjects;
using Assets.Scripts.Data;
using Assets.Scripts.Managers.Interfaces;
using System;
using System.Linq;

namespace Assets.Scripts.Managers
{
    public class BackgroundManager : IBackgroundManager
    {
        private BackgroundConfig _currentBackgroundConfig;
        private BackgroundData _backgroundData;

        private BackgroundConfig[] _backgroundConfigs;

        public BackgroundManager(GameResources gameResources)
        {
            _backgroundConfigs = gameResources.Backgrounds.ToArray();
        }

        public BackgroundData GetBackgroundByLevelIndex(int levelIndex)
        {
            var levelId = levelIndex + 1;

            if (!IsLevelIndexInBoard(_currentBackgroundConfig, levelId))
            {

                var config = _backgroundConfigs.Where(config => IsLevelIndexInBoard(config, levelId))
                                  .First();

                if (config.Sprite != null)
                {
                    _currentBackgroundConfig = config;
                }
                else
                {
                    //Can be loaded default background
                    throw new ArgumentException($"No background for {levelId}");
                }

                _backgroundData = new BackgroundData()
                {
                    Sprite = _currentBackgroundConfig.Sprite,
                    BoardConfig = _currentBackgroundConfig.BoardConfig,
                };
            }

            return _backgroundData;
        }

        private bool IsLevelIndexInBoard(BackgroundConfig config, int levelId) 
            => config.StartLevelId <= levelId && config.EndLevelId >= levelId;
    }
}
