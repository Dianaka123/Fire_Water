using UnityEngine;

namespace Assets.Scripts.ScriptableObjects
{
    [CreateAssetMenu(fileName = "LevelsConfiguration", menuName = "ScriptableObjects/LevelsConfiguration", order = 1)]
    public class LevelsConfiguration : ScriptableObject
    {
        public TextAsset LevelsJSON;
    }


}