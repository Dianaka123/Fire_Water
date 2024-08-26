using Assets.Scripts.Managers.Interfaces;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Views
{
    public class UIManager : MonoBehaviour, IUIManger
    {
        [SerializeField]
        private RectTransform _dynamicCanvasRect;

        [SerializeField]
        private Transform _blockRoot;

        [SerializeField]
        private Transform _ballonRoot;

        [SerializeField]
        private Image _backgroundImage;

        [SerializeField]
        private LevelManipulationView _levelManipulationView;

        public Vector2 Size => _dynamicCanvasRect.rect.size;
        public Transform BlocksRoot => _blockRoot;
        public Transform BallonRoot => _ballonRoot;

        public event Action Restart;
        public event Action Next;

        public void SetBackground(Sprite sprite)
        {
            _backgroundImage.sprite = sprite;
        }

        private void Awake()
        {
            _levelManipulationView.RefreshLevel += () => Restart?.Invoke();
            _levelManipulationView.NextLevel += () => Next?.Invoke();
        }
    }
}
