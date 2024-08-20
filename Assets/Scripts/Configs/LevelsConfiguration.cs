using UnityEngine;
using System.Collections.Generic;
using System;

namespace Assets.Scripts.Configs
{
    [CreateAssetMenu(fileName = "LevelsConfiguration", menuName = "ScriptableObjects/LevelsConfiguration", order = 1)]
    public class LevelsConfiguration: ScriptableObject
    {
        public List<Sprite> Bloks;
        public List<Sprite> Backgrounds;
        public TextAsset LevelsJSON;
    }
}