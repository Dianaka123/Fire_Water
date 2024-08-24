using Assets.Scripts.Managers.Interfaces;
using Assets.Scripts.Services.Interfaces;
using UnityEngine;

namespace Assets.Scripts.Services
{
    public struct SwipeState
    {
        public Direction Direction;
        public bool IsSwiping;
        public Vector3 startPosition;
    }

    public class InputSystem : IInputSystem
    {
        private Direction _direction = Direction.None;
        private Touch? _initialTouch;
        private bool _swiping;

        private float minSwipeDistance;
        private float errorRange;

        public SwipeState? CheckSwipe()
        {
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                
                if (touch.phase == TouchPhase.Began)
                {
                    _initialTouch = touch;
                }
                else if (touch.phase == TouchPhase.Moved && _initialTouch.HasValue)
                {
                    var deltaX = touch.position.x - _initialTouch.Value.position.x; //greater than 0 is right and less than zero is left
                    var deltaY = touch.position.y - _initialTouch.Value.position.y; //greater than 0 is up and less than zero is down
                    var swipeDistance = Mathf.Abs(deltaX) + Mathf.Abs(deltaY);

                    if (swipeDistance > minSwipeDistance && (Mathf.Abs(deltaX) > 0 || Mathf.Abs(deltaY) > 0))
                    {
                        _swiping = true;

                        CalculateSwipeDirection(deltaX, deltaY);
                    }
                }
                else if (touch.phase == TouchPhase.Ended)
                {
                    var result = new SwipeState()
                    {
                        Direction = _direction,
                        IsSwiping = _swiping,
                        startPosition = _initialTouch.Value.position,
                    };

                    _initialTouch = null;
                    _swiping = false;
                    _direction = Direction.None;

                    return result.IsSwiping ? result : null;
                }
                else if (touch.phase == TouchPhase.Canceled)
                {
                    _initialTouch = null;
                    _swiping = false;
                    _direction = Direction.None;
                }
            }

            return null;
        }

        void CalculateSwipeDirection(float deltaX, float deltaY)
        {
            bool isHorizontalSwipe = Mathf.Abs(deltaX) > Mathf.Abs(deltaY);

            // horizontal swipe
            if (isHorizontalSwipe && Mathf.Abs(deltaY) <= errorRange)
            {
                //right
                if (deltaX > 0)
                    _direction = Direction.Right;
                //left
                else if (deltaX < 0)
                    _direction = Direction.Left;
            }
            //vertical swipe
            else if (!isHorizontalSwipe && Mathf.Abs(deltaX) <= errorRange)
            {
                //up
                if (deltaY > 0)
                    _direction = Direction.Up;
                //down
                else if (deltaY < 0)
                    _direction = Direction.Down;
            }
            //diagonal swipe
            else
            {
                _swiping = false;
            }
        }
    }
}
