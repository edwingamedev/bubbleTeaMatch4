using UnityEngine;

namespace EdwinGameDev.BubbleTeaMatch4
{
    public class TouchInputProvider
    {
        private Vector2 fingerUp;
        private Vector2 fingerDown;
        private float swipeHorizontalThreshold = 30f;
        private float swipeVerticalThreshold = 10f;
        private InputType movedInput = InputType.NoInput;

        private float touchBegan = 0;
        private float releaseDelay = 0.5f;

        public InputType DetectTouch()
        {
            // Finger on screen
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                movedInput = InputType.NoInput;

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

                    if (movedInput != InputType.NoInput)
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
                            return InputType.Release;
                        }
                        else
                        {
                            return InputType.Click;
                        }
                    }
                    else
                    {
                        // Click
                        return InputType.Click;
                    }
                }

                // No Touch
                return movedInput;
            }
            // No Movement
            else
            {
                return InputType.NoInput;
            }
        }

        private InputType DetectSwipe(Vector2 touchPosition)
        {
            // Vertical Movement
            if (VerticalMove() > HorizontalMove() && VerticalMove() > swipeVerticalThreshold)
            {
                //fingerDown = touchPosition;

                if (fingerDown.y - fingerUp.y > 0)// Swipe Up
                {
                    fingerUp = fingerDown;
                    return InputType.Up;
                }
                else if (fingerDown.y - fingerUp.y < 0)// Swipe Down
                {
                    fingerUp = fingerDown;
                    return InputType.Down;
                }

                fingerUp = fingerDown;

                // No Movement
                return InputType.NoInput;

            }
            // Horizontal
            else if (HorizontalMove() > VerticalMove() && HorizontalMove() > swipeHorizontalThreshold)
            {
                //fingerDown = touchPosition;

                if (fingerDown.x - fingerUp.x > 0)// Swipe Right
                {
                    fingerUp = fingerDown;
                    return InputType.Right;
                }
                else if (fingerDown.x - fingerUp.x < 0)// Swipe Left
                {
                    fingerUp = fingerDown;
                    return InputType.Left;
                }

                fingerUp = fingerDown;

                // No Movement
                return InputType.NoInput;
            }
            // No Movement
            else
            {
                return InputType.NoInput;
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