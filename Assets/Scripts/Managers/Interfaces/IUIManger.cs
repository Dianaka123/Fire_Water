using System;
using UnityEngine;

namespace Assets.Scripts.Managers.Interfaces
{
    public interface IUIManger
    {
        event Action Restart;
        event Action Next;

        Vector2 Size { get; }
        Transform BlocksRoot { get; }
        Transform BallonRoot { get; }

        public void SetBackground(Sprite sprite);
    }
}
