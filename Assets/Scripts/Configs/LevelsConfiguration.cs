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

    
}