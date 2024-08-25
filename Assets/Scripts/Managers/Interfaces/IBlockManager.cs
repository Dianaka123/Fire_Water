using Assets.Scripts.Services.Data;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Managers.Interfaces
{
    public interface IBlockManager
    {
        void CreateBlocks(int[,] level, GridData grid, Transform parent);
        UniTask SwitchBlocksAsync(Vector2Int cellFrom, Vector2Int cellTo, Vector3 startPosition, Vector3 endPosition);
    }
}
