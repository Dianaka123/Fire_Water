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

        //TODO: move to config
        private float minSwipeDistance = 1;
        private float errorRange = 100;

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
                    var deltaX = touch.position.x - _initialTouch.Value.position.x;
                    var deltaY = touch.position.y - _initialTouch.Value.position.y;
                    var swipeDistance = Mathf.Abs(deltaX) + Mathf.Abs(deltaY);

                    if (swipeDistance > minSwipeDistance && (Mathf.Abs(deltaX) > 0 || Mathf.Abs(deltaY) > 0))
                    {
                        _swiping = true;
                        CalculateSwipeDirection(deltaX, deltaY);
                    }
                }
                else if (touch.phase == TouchPhase.Ended)
                {
                    if(!_initialTouch.HasValue)
                    { 
                        return null;
                    }

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
