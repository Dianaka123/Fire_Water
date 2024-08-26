using Assets.Scripts.Services;
using Assets.Scripts.Wrappers;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using Zenject;

namespace Assets.Scripts.Tests
{
    public class NormilizeBordTest : ZenjectUnitTestFixture
    {
        [Inject]
        private BoardNormalizer normalizator;

        [SetUp]
        public void Init()
        {
            Container.BindInterfacesAndSelfTo<BoardNormalizer>().AsSingle();
            Container.Inject(this);
        }

        [Test]
        public void Normilization()
        {
            var array = new int[8]
            {
                0, 0, -1, - 1,
                0, 1, 1, 1
            };
            var array2D = new Array2D<int>(array, 2, 4);

            var a = normalizator.GetBlockSequenceForDestroying(array2D, -1);
            Assert.AreEqual(3, a.Length);
        }

        [Test]
        public void Normilization2()
        {
            var array = new int[]
            {
                0, -1, -1, 1,
                0, 1, 1, 1
            };

            var array2D = new Array2D<int>(array, 2, 4);

            var a = normalizator.GetBlockSequenceForDestroying(array2D, -1);
            Assert.AreEqual(4, a.Length);
        }
    }
}
