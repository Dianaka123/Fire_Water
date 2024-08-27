using Assets.Scripts.Services;
using Assets.Scripts.Wrappers;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.Tests
{
    public class BoardNormalizerTests : ZenjectUnitTestFixture
    {
        private const int EmptyCell = -1;
        public struct LevelTestCases
        {
            public Array2D<int> Level;
            public int SequenceResult;
        };

        private static List<LevelTestCases> Levels = new()
        {
           new LevelTestCases
           {
               Level = new Array2D<int>(new int[]
                        {
                            0, 0, EmptyCell, EmptyCell,
                            0, 1, 1, 1
                        }, 2, 4),

               SequenceResult = 3,
           },
           new LevelTestCases
           {
               Level = new Array2D<int>(new int[8]
                        {
                            0, EmptyCell, EmptyCell, 1,
                            0, 1, 1, 1
                        },2, 4),
               SequenceResult = 4,
           },
           new LevelTestCases
           {
               Level = new Array2D<int>(new int[8]
                        {
                            0, EmptyCell, EmptyCell , 1,
                            0, EmptyCell, 1, 1
                        },2, 4),
               SequenceResult = 0,
           },
        };

        [Inject]
        private BoardNormalizer _normalizator;

        [SetUp]
        public void Init()
        {
            Container.BindInterfacesAndSelfTo<BoardNormalizer>().AsSingle();
            Container.Inject(this);
        }

        [TestCaseSource("Levels")]
        public void GetBlockSequenceForDestroying_FindSequenceLength(LevelTestCases testCase)
        {
            Vector2Int[] sequence = _normalizator.GetBlockSequenceForDestroying(testCase.Level, EmptyCell);
            Assert.AreEqual(testCase.SequenceResult, sequence.Length);
        }
    }
}
