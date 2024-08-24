using UnityEngine;

namespace Assets.Scripts.Services.Interfaces
{
    public enum Direction
    {
        Up,
        Down, 
        Left, 
        Right,
        None
    }

    public interface IInputSystem
    {
        SwipeState? CheckSwipe();
    }
}
