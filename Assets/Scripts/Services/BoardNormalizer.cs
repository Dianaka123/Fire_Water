using Assets.Scripts.Services.Interfaces;
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

        public Vector2Int[] GetBlockSequenceForDestroying(int[,] levelSequence, int emptyCellId)
        {
            var result = new HashSet<Vector2Int>();
            var visitedIndexes = new HashSet<Vector2Int>();
            for (var i = 0; i < levelSequence.GetLength(0); i++)
            {
                for (var j = 0; j < levelSequence.GetLength(1); j++)
                {
                    var currentIndex = new Vector2Int(i, j);
                    if (levelSequence[i, j] == emptyCellId || visitedIndexes.Contains(currentIndex))
                    {
                        continue;
                    }
                    Debug.Log("-----------");
                    var startSequence = new Sequence() { HorizontalCounter = 1, VerticalCounter = 1, VisitedIndexes = new() };

                    var resultSequence = FindSequence(levelSequence, i, j, levelSequence[i, j], startSequence, 0, 0);

                    foreach (var index in resultSequence.VisitedIndexes)
                    {
                        visitedIndexes.Add(index);
                    }

                    if (resultSequence.HorizontalCounter >= MinMatchCount || resultSequence.VerticalCounter >= MinMatchCount)
                    {
                        foreach(var index in resultSequence.VisitedIndexes)
                        {
                            result.Add(index);
                        }
                    }
                }
            }

            return result.ToArray();
        }

        private Sequence FindSequence(int[,] levelSequence, int x, int y, int value, Sequence sequence, int h, int v)
        {
            if(y < 0 
                || x < 0 
                || y >= levelSequence.GetLength(1) 
                || x >= levelSequence.GetLength(0)
                || levelSequence[x, y] != value
                || sequence.VisitedIndexes.Contains(new Vector2Int(x, y)))
            {
                return sequence;
            }

            //Debug.Log($"{x} - {y}");

            sequence = new Sequence()
            {
                VisitedIndexes = sequence.VisitedIndexes,
                HorizontalCounter = sequence.HorizontalCounter + h,
                VerticalCounter = sequence.VerticalCounter + v
            };

            sequence.VisitedIndexes.Add(new Vector2Int(x, y));

            var left = y - 1;
            var leftSeq = FindSequence(levelSequence, x, left, value, new Sequence()
            {
                VisitedIndexes = sequence.VisitedIndexes,
                HorizontalCounter = sequence.HorizontalCounter,
                VerticalCounter = 1,
            }, 1, 0);
            
            var right = y + 1;
            var rightSeq = FindSequence(levelSequence, x, right, value, new Sequence()
            {
                VisitedIndexes = sequence.VisitedIndexes,
                HorizontalCounter = sequence.HorizontalCounter,
                VerticalCounter = 1,
            }, 1, 0);

            var top = x + 1;
            var topSeq = FindSequence(levelSequence, top, y, value, new Sequence()
            {
                VisitedIndexes = sequence.VisitedIndexes,
                HorizontalCounter = 1,
                VerticalCounter = sequence.VerticalCounter,
            }, 0, 1);

            var bottom = x - 1;
            var bottomSeq = FindSequence(levelSequence, bottom, y, value, new Sequence()
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
