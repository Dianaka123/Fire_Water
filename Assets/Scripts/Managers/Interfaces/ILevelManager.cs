using Assets.Scripts.Configs;
using Assets.Scripts.Services.Data;

namespace Assets.Scripts.Managers.Interfaces
{
    public interface ILevelManager
    {
        public LevelDesc CurrentLevel { get; }

        void UpdateLevelBySavingData(LevelSavingData levelSavingData);
    }
}
