using UnityEngine;
using System.Collections.Generic;
using Assets.Scripts.Views;

namespace Assets.Scripts.Configs
{
    [CreateAssetMenu(fileName = "LevelsConfiguration", menuName = "ScriptableObjects/LevelsConfiguration", order = 1)]
    public class LevelsConfiguration: ScriptableObject
    {
        public List<Block> Bloks;
        public List<Sprite> Backgrounds;
        public TextAsset LevelsJSON;
    }
}