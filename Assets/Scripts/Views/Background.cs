
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Views
{
    public class Background
    {
        [SerializeField]
        private Image Image;

        public void SetBackground(Sprite sprite)
        {
            Image.sprite = sprite;
        }
    }
}
