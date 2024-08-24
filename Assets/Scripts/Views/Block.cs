﻿using UnityEngine;
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

        public void SetSize(float cellSize)
        {
            var scaledSize = cellSize * _scaleRatio;
            _image.rectTransform.sizeDelta = new Vector2 (scaledSize, scaledSize);
        }
    }
}