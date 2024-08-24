using UnityEngine;
using System.Collections.Generic;
using Assets.Scripts.Views;
using System;

namespace Assets.Scripts.Configs
{
    [CreateAssetMenu(fileName = "LevelsConfiguration", menuName = "ScriptableObjects/LevelsConfiguration", order = 1)]
    public class LevelsConfiguration: ScriptableObject
    {
        public TextAsset LevelsJSON;
    }

    [CreateAssetMenu(fileName = "GameResources", menuName = "ScriptableObjects/GameResources", order = 1)]
    public class GameResources : ScriptableObject
    {
        public List<Block> Bloks;
        public List<BackgroundConfig> Backgrounds;
    }

    [Serializable]
    public class BackgroundConfig
    {
        public Sprite Sprite;
        public BoardConfig BoardConfig;
    }

    [Serializable]
    public struct BoardConfig
    {
        public float SideOffset;
        public float BottomOffset;
    }
}