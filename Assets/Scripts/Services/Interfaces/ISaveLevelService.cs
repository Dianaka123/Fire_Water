using Assets.Scripts.Data;
using Cysharp.Threading.Tasks;
using System.Threading;

namespace Assets.Scripts.Services.Interfaces
{
    public interface ISaveLevelService
    {
        UniTask SaveLevelStateAsync(CancellationToken cancellationToken = default);
        UniTask<Level> GetSavedDataAsync(CancellationToken cancellationToken = default);
    }
}
