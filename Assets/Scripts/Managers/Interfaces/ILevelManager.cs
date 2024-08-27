using Assets.Scripts.Data;
using Assets.Scripts.Wrappers;
using UnityEngine;

namespace Assets.Scripts.Managers.Interfaces
{
    public interface ILevelManager
    {
        Array2D<int> CurrentLevelSequence { get; }
        int CurrentLevelIndex { get; }
        int EmptyCellId { get; }

        void UpdateLevel(Level levelSavingData);
        void SwitchBlocks(Vector2Int from, Vector2Int to);
        void NextLevel();
        void RestartLevel();

        bool IsEmptyCell(Vector2Int cellIndex);
        void SetEmptyCell(Vector2Int cellIndex);
        bool IsLevelCompleted();
    }
}
