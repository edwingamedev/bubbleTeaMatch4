using UnityEngine;

namespace EdwinGameDev.BubbleTeaMatch4
{
    public class TouchInputProvider
    {
        private Vector2 fingerUp;
        private Vector2 fingerDown;
        private float swipeHorizontalThreshold = 30f;
        private float swipeVerticalThreshold = 10f;
        private TouchInputType movedInput = TouchInputType.NoInput;

        private float touchBegan = 0;
        private float releaseDelay = 0.5f;

        public TouchInputType DetectTouch()
        {
            // Finger on screen
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                movedInput = TouchInputType.NoInput;

                // Finger touched
                if (touch.phase == TouchPhase.Began)
                {
                    // Touched on screen
                    touchBegan = Time.time;

                    fingerUp = touch.position;
                    fingerDown = touch.position;
                }

                // Finger moved
                if (touch.phase == TouchPhase.Moved)
                {
                    fingerDown = touch.position;

                    // Detect swipe when moving
                    movedInput = DetectSwipe(touch.position);

                    if (movedInput != TouchInputType.NoInput)
                        return movedInput;
                }

                // Finger released
                if (touch.phase == TouchPhase.Ended)
                {
                    fingerUp = touch.position;

                    // Valid Release
                    if (Time.time - touchBegan < releaseDelay)
                    {
                        if (fingerUp == fingerDown)
                        {
                            return TouchInputType.Release;
                        }
                        else
                        {
                            return TouchInputType.Click;
                        }
                    }
                    else
                    {
                        // Click
                        return TouchInputType.Click;
                    }
                }

                // No Touch
                return movedInput;
            }
            // No Movement
            else
            {
                return TouchInputType.NoInput;
            }
        }

        private TouchInputType DetectSwipe(Vector2 touchPosition)
        {
            // Vertical Movement
            if (VerticalMove() > HorizontalMove() && VerticalMove() > swipeVerticalThreshold)
            {
                //fingerDown = touchPosition;

                if (fingerDown.y - fingerUp.y > 0)// Swipe Up
                {
                    fingerUp = fingerDown;
                    return TouchInputType.Up;
                }
                else if (fingerDown.y - fingerUp.y < 0)// Swipe Down
                {
                    fingerUp = fingerDown;
                    return TouchInputType.Down;
                }

                fingerUp = fingerDown;

                // No Movement
                return TouchInputType.NoInput;

            }
            // Horizontal
            else if (HorizontalMove() > VerticalMove() && HorizontalMove() > swipeHorizontalThreshold)
            {
                //fingerDown = touchPosition;

                if (fingerDown.x - fingerUp.x > 0)// Swipe Right
                {
                    fingerUp = fingerDown;
                    return TouchInputType.Right;
                }
                else if (fingerDown.x - fingerUp.x < 0)// Swipe Left
                {
                    fingerUp = fingerDown;
                    return TouchInputType.Left;
                }

                fingerUp = fingerDown;

                // No Movement
                return TouchInputType.NoInput;
            }
            // No Movement
            else
            {
                return TouchInputType.NoInput;
            }
        }

        private float VerticalMove()
        {
            return Mathf.Abs(fingerDown.y - fingerUp.y);
        }

        private float HorizontalMove()
        {
            return Mathf.Abs(fingerDown.x - fingerUp.x);
        }
    }
}