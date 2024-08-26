using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Views
{
    public class BalloonView : MonoBehaviour
    {
        [SerializeField]
        private Image _image;

        [SerializeField]
        private RectTransform _rectTransform;

        public float Speed;

        public void SetPosition(Vector2 position)
        {
            _rectTransform.localPosition = position;
        }

        public void SetImage(Sprite sprite)
        {
            _image.sprite = sprite;
            _rectTransform.sizeDelta = sprite.textureRect.size;
        }
     
        public void ResizeImage(float size)
        {
            var originalSize = _rectTransform.sizeDelta;
            var scale = size / originalSize.x;
            _rectTransform.sizeDelta = new Vector2(scale * originalSize.x, scale * originalSize.y);
        }

        public void SetActive(bool isActive)
        {
            gameObject.SetActive(isActive);
        }
    }
}
