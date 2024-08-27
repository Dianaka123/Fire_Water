using Assets.Scripts.Data;
using Assets.Scripts.Views;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.ScriptableObjects
{
    [CreateAssetMenu(fileName = "GameResources", menuName = "ScriptableObjects/GameResources", order = 1)]
    public class GameResources : ScriptableObject
    {
        public List<Block> Bloks;
        public List<BackgroundConfig> Backgrounds;
        public BalloonConfig Balloons;
    }

    
}
