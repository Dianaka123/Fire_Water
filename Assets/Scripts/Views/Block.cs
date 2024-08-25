using Cysharp.Threading.Tasks;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Views
{
    [RequireComponent(typeof(Animator))]
    public class Block : MonoBehaviour
    {
        [SerializeField]
        private Animator _animator;

        [SerializeField]
        private Image _image;

        [SerializeField]
        private float _scaleRatio = 1.6f;

        [SerializeField]
        private RectTransform _rectTransform;

        public int SiblingIndex { 
            get
            {
                return transform.GetSiblingIndex();
            }

            set
            {
                if(value >= 0)
                {
                    transform.SetSiblingIndex(value);
                }
            }
        }

        public void SetSize(float cellSize)
        {
            var scaledSize = cellSize * _scaleRatio;
            _image.rectTransform.sizeDelta = new Vector2 (scaledSize, scaledSize);
        }

        public UniTask AnimateMovingAsync(Vector3 to, float duration)
        {
            var previousPosition = gameObject.transform.localPosition;

            float time = 0.0f;
            return UniTask.WaitUntil(() =>
            {
                time += Time.deltaTime;
                gameObject.transform.localPosition = Vector3.Lerp(previousPosition, to, time / duration);
                return time > duration;
            });
        }
    }
}