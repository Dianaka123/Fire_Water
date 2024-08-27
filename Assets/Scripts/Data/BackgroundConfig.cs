using System;
using UnityEngine;

namespace Assets.Scripts.Data
{
    [Serializable]
    public struct BackgroundConfig
    {
        public Sprite Sprite;
        public BoardConfig BoardConfig;

        //TODO: Constrain for min max
        public int StartLevelId;
        public int EndLevelId;
    }
}
