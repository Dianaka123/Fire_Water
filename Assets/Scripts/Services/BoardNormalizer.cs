using Assets.Scripts.Services.Interfaces;
using Assets.Scripts.Wrappers;
using Codice.Client.BaseCommands.BranchExplorer;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Services
{
    public class BoardNormalizer : IBoardNormalizer
    {
        private const int MinMatchCount = 3;

        private struct Sequence
        {
            public HashSet<Vector2Int> VisitedIndexes;
            public int HorizontalCounter;
            public int VerticalCounter;
        }

        public Vector2Int[] GetBlockSequenceForDestroying(Array2D<int> levelSequence, int emptyCellId)
        {
            var result = new HashSet<Vector2Int>();
            var visitedIndexes = new HashSet<Vector2Int>();
            levelSequence.ForEach(index =>
            {
                var currentIndex = index;
                if (levelSequence[index] == emptyCellId || visitedIndexes.Contains(currentIndex))
                {
                    return;
                }

                var startSequence = new Sequence() { HorizontalCounter = 1, VerticalCounter = 1, VisitedIndexes = new() };

                var resultSequence = FindSequence(levelSequence, index, levelSequence[index], startSequence, 0, 0);

                foreach (var cell in resultSequence.VisitedIndexes)
                {
                    visitedIndexes.Add(cell);
                }

                if (resultSequence.HorizontalCounter >= MinMatchCount || resultSequence.VerticalCounter >= MinMatchCount)
                {
                    foreach (var cell in resultSequence.VisitedIndexes)
                    {
                        result.Add(cell);
                    }
                }
            });

            return result.ToArray();
        }

        private Sequence FindSequence(Array2D<int> levelSequence, Vector2Int index, int value, Sequence sequence, int h, int v)
        {
            if(index.y < 0 
                || index.x < 0 
                || index.y >= levelSequence.RowCount 
                || index.x >= levelSequence.ColumnCount
                || levelSequence[index] != value
                || sequence.VisitedIndexes.Contains(index))
            {
                return sequence;
            }

            sequence = new Sequence()
            {
                VisitedIndexes = sequence.VisitedIndexes,
                HorizontalCounter = sequence.HorizontalCounter + h,
                VerticalCounter = sequence.VerticalCounter + v
            };

            sequence.VisitedIndexes.Add(index);

            var left = index + Vector2Int.left;
            var leftSeq = FindSequence(levelSequence, left, value, new Sequence()
            {
                VisitedIndexes = sequence.VisitedIndexes,
                HorizontalCounter = sequence.HorizontalCounter,
                VerticalCounter = 1,
            }, 1, 0);
            
            var right = index + Vector2Int.right;
            var rightSeq = FindSequence(levelSequence, right, value, new Sequence()
            {
                VisitedIndexes = sequence.VisitedIndexes,
                HorizontalCounter = sequence.HorizontalCounter,
                VerticalCounter = 1,
            }, 1, 0);

            var top = index + Vector2Int.up;
            var topSeq = FindSequence(levelSequence, top, value, new Sequence()
            {
                VisitedIndexes = sequence.VisitedIndexes,
                HorizontalCounter = 1,
                VerticalCounter = sequence.VerticalCounter,
            }, 0, 1);

            var bottom = index + Vector2Int.down;
            var bottomSeq = FindSequence(levelSequence, bottom, value, new Sequence()
            {
                VisitedIndexes = sequence.VisitedIndexes,
                HorizontalCounter = 1,
                VerticalCounter = sequence.VerticalCounter,
            }, 0, 1);

            var horizontalCounter = Math.Max(leftSeq.HorizontalCounter, rightSeq.HorizontalCounter);
            horizontalCounter = Math.Max(horizontalCounter, topSeq.HorizontalCounter);
            horizontalCounter = Math.Max(horizontalCounter, bottomSeq.HorizontalCounter);
            horizontalCounter = Math.Max(horizontalCounter, sequence.HorizontalCounter);

            var verticalCounter = Math.Max(leftSeq.VerticalCounter, rightSeq.VerticalCounter);
            verticalCounter = Math.Max(verticalCounter, topSeq.VerticalCounter);
            verticalCounter = Math.Max(verticalCounter, bottomSeq.VerticalCounter);
            verticalCounter = Math.Max(verticalCounter, sequence.VerticalCounter);

            return new Sequence() {
                VisitedIndexes = sequence.VisitedIndexes,
                HorizontalCounter = horizontalCounter,
                VerticalCounter = verticalCounter 
            };
        }

        private int GoHorizontal(int[,] levelSequence, int x, int y, int count)
        {
            var newY = y + 1;

            if (newY > 0 && newY < levelSequence.GetLength(1))
            {
                if (levelSequence[x, y] == levelSequence[x, newY])
                {
                    Debug.Log($"{x} {y}");
                    return GoHorizontal(levelSequence, x, newY, count + 1);
                }
            }

            return count;
        }
    }
}
