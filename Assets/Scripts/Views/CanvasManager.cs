using Assets.Scripts.Managers.Interfaces;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Views
{
    public class CanvasManager : MonoBehaviour, ICanvasManger
    {
        [SerializeField]
        private RectTransform _dynamicRectTransform;

        [SerializeField]
        private Image _backgroundImage;

        public Vector2 Size => _dynamicRectTransform.rect.size;
        public Transform DynamicCanvasTransform => _dynamicRectTransform.transform;

        public void SetBackground(Sprite sprite)
        {
            _backgroundImage.sprite = sprite;
        }
    }
}
