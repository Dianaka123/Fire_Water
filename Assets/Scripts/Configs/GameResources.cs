using Assets.Scripts.Views;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Configs
{
    [CreateAssetMenu(fileName = "GameResources", menuName = "ScriptableObjects/GameResources", order = 1)]
    public class GameResources : ScriptableObject
    {
        public List<Block> Bloks;
        public List<BackgroundConfig> Backgrounds;
        public BalloonConfig Balloon;
    }

    [Serializable]
    public class BackgroundConfig
    {
        public Sprite Sprite;
        public BoardConfig BoardConfig;
        
        //TODO: Constrain for min max
        public int StartLevelId;
        public int EndLevelId;
    }

    [Serializable]
    public struct BoardConfig
    {
        [Range(0, 1)]
        public float RelativeSideOffset;
        [Range(0, 1)]
        public float RelativeBottomOffset;
    }

    [Serializable]
    public struct BalloonConfig
    {
        public List<Sprite> Sprites;
        [Range(0, 10)]
        public int TotalCount;

        public BalloonView Prefab;

        [Range(0, 1)]
        public float RelativeSize;

        [Min(0.1f)]
        public float Duration;

        //TODO: Constrain for min max
        public float MinSpeed;
        public float MaxSpeed;

    }
}
