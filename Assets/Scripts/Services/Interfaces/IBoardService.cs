using Assets.Scripts.Configs;
using UnityEngine;

namespace Assets.Scripts.Services.Interfaces
{
    public interface IBoardService
    {
        Vector2[,] Indexes { get; }
        void CreateBoardForLevel(Vector2Int gridSize, BoardConfig boardConfig);
    }
}
