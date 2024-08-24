using UnityEngine;

namespace Assets.Scripts.Managers.Interfaces
{
    public interface ICanvasManger
    {
        Vector2 Size { get; }
        Transform DynamicCanvasTransform { get; }

        public void SetBackground(Sprite sprite);
    }
}
