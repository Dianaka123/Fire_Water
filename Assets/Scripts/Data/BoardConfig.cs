using System;
using UnityEngine;

namespace Assets.Scripts.Data
{
    [Serializable]
    public struct BoardConfig
    {
        [Range(0, 1)]
        public float RelativeSideOffset;
        [Range(0, 1)]
        public float RelativeBottomOffset;
    }
}
