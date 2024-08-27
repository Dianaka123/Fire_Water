using Assets.Scripts.Data;

namespace Assets.Scripts.Managers.Interfaces
{
    public interface IBackgroundManager
    {
        BackgroundData GetBackgroundByLevelIndex(int levelIndex);
    }
}
