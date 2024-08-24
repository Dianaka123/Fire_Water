using Assets.Scripts.Configs;

namespace Assets.Scripts.Managers.Interfaces
{
    public interface ILevelManager
    {
        public Level CurrentLevel { get; }

        void UpdateLevelBySavingData(Level levelSavingData);
    }
}
