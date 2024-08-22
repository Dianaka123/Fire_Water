using UnityEngine;

namespace Assets.Scripts.Views
{
    public class Board : MonoBehaviour
    {
        [SerializeField]
        private RectTransform _rectTransform;

        public Vector2 CanvasSize => _rectTransform.rect.size;
        public Vector3 CanvasScale => _rectTransform.localScale;
    }
}
