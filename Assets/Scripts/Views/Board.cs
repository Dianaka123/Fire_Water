using Assets.Scripts.Managers;
using UnityEngine;

namespace Assets.Scripts.Views
{
    public class Board : MonoBehaviour, ICanvasManger
    {
        [SerializeField]
        private RectTransform _rectTransform;

        public Vector2 Size => _rectTransform.rect.size;
        public Vector3 LocalScale => _rectTransform.localScale;
    }
}
