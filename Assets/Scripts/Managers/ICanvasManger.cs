using UnityEngine;

namespace Assets.Scripts.Managers
{
    public interface ICanvasManger
    {
        Vector2 Size { get; }
        Vector3 LocalScale { get; }
    }
}
