using Assets.Scripts.Services.Data;
using Assets.Scripts.Wrappers;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Managers.Interfaces
{
    public interface IBlocksManager
    {
        void CreateBlocks(Array2D<int> level, GridData grid, Transform parent);
        void RestartLevel(Array2D<int> level, GridData grid, Transform parent);
        UniTask SwitchBlocksAsync(Vector2Int cellFrom, Vector2Int cellTo, Vector3 startPosition, Vector3 endPosition);
        UniTask DestroyBlocksAsync(Vector2Int[] indexes);
    }
}
