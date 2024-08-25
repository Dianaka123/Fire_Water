using Assets.Scripts.Services;
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
            var array = new int[2, 4]
            {
                {0, 1, 1, 1, },
                {0, 0, -1, -1, }
            };

            var a = normalizator.GetBlockSequenceForDestroying(array, -1);
            Assert.AreEqual(3, a.Length);
        }

        [Test]
        public void Normilization2()
        {
            var array = new int[,]
            {
                {0, -1, -1, 1, },
                {0, 1, 1, 1, }
            };

            var a = normalizator.GetBlockSequenceForDestroying(array, -1);
            Assert.AreEqual(4, a.Length);
        }
    }
}
