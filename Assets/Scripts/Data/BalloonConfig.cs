using Assets.Scripts.Views;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Data
{
    [Serializable]
    public struct BalloonConfig
    {
        public List<Sprite> Sprites;
        [Range(0, 10)]
        public int TotalCount;

        public BalloonView Prefab;

        [Range(0, 1)]
        public float RelativeSize;

        [Range(0, 1)]
        public float RelativeBottomOffset;

        [Min(0.1f)]
        public float Duration;

        //TODO: Constrain for min max
        public float MinSpeed;
        public float MaxSpeed;

    }
}
