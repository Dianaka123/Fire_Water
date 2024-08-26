using Assets.Scripts.Configs;
using Assets.Scripts.Wrappers;
using UnityEngine;

namespace Assets.Scripts.Managers.Interfaces
{
    public interface ILevelManager
    {
        Array2D<int> CurrentLevelSequence { get; }
        int CurrentLevelId { get; }
        int EmptyCellId { get; }

        void UpdateLevelBySavingData(Level levelSavingData);
        void SwitchBlocks(Vector2Int from, Vector2Int to);
        bool IsEmptyCell(Vector2Int cellIndex);
        void SetEmptyCell(Vector2Int cellIndex);
        bool IsLevelCompleted();
        void NextLevel();
        void RestartLevel();
    }
}
