using Assets.Scripts.Configs;
using Cysharp.Threading.Tasks;
using System.Threading;

namespace Assets.Scripts.Services.Interfaces
{
    public interface ISaveLevelService
    {
        UniTask SaveLevelStateAsync(CancellationToken cancellationToken);
        UniTask<Level> GetSavedDataAsync(CancellationToken cancellationToken);
    }
}
