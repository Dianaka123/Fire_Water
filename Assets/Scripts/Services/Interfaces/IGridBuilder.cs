using Assets.Scripts.Data;
using Assets.Scripts.Services.Data;
using UnityEngine;

namespace Assets.Scripts.Services.Interfaces
{
    public interface IGridBuilder
    {
        GridData CreateGridForLevel(Vector2Int gridSize, BoardConfig boardConfig, Vector2 windowSize);
    }
}
