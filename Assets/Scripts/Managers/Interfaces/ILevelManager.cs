using Assets.Scripts.Configs;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Managers.Interfaces
{
    public interface ILevelManager
    {
        int[,] CurrentLevelSequence { get; }
        int EmptyCellId { get; }


        void UpdateLevelBySavingData(Level levelSavingData);
        void SwitchBlocks(Vector2Int from, Vector2Int to);
        bool IsEmptyCell(Vector2Int cellIndex);
        void SetEmptyCell(Vector2Int cellIndex);
        bool IsLevelCompleted();
        void NextLevel();
    }
}
