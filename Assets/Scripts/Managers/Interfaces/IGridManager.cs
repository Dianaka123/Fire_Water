using Assets.Scripts.Configs;
using Assets.Scripts.Services.Data;
using UnityEngine;

namespace Assets.Scripts.Managers.Interfaces
{
    public interface IGridManager
    {
        GridData CreateGrid(Vector2Int gridSize, BoardConfig boardConfig);
        Vector2Int GetCellIndexByScreenPosition(Vector3 screenPosition);
        Vector2 GetScreenPositionByCellIndex(Vector2Int cellIndex);
    }
}
