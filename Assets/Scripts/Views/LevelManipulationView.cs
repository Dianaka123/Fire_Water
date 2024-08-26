using System;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Views
{
    public class LevelManipulationView : MonoBehaviour
    {
        [SerializeField]
        private Button _refreshLevelBtn;

        [SerializeField]
        private Button _nextLevel;

        public event Action RefreshLevel;
        public event Action NextLevel;

        private void Awake()
        {
            _refreshLevelBtn.onClick.AddListener(() => RefreshLevel?.Invoke());
            _nextLevel.onClick.AddListener(() => NextLevel?.Invoke());
        }

        private void OnDestroy()
        {
            _refreshLevelBtn.onClick.RemoveAllListeners();
            _nextLevel.onClick.RemoveAllListeners();
        }
    }
}
