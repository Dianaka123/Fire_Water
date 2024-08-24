using Assets.Scripts.Services.Data;
using Cysharp.Threading.Tasks;
using System.Threading;

namespace Assets.Scripts.Services.Interfaces
{
    public interface ISaveLevelService
    {
        UniTask SaveLevelStateAsync(CancellationToken cancellationToken);
        UniTask<LevelSavingData> GetSavedDataAsync(CancellationToken cancellationToken);
    }
}
