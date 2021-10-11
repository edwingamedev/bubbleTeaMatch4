using System;
using UnityEngine;

namespace EdwinGameDev.BubbleTeaMatch4
{
    public class TouchInputProcessor : IInputProcessor
    {
        private TouchInputProvider touchInputProcessor;
        public Action OnTurnClockwise { get; set; }
        public Action OnTurnCounterClockwise { get; set; }
        public Action<Vector2Int> OnMove { get; set; }

        public TouchInputProcessor()
        {
            touchInputProcessor = new TouchInputProvider();
        }

        public void CheckInputs()
        {
            var touch = touchInputProcessor.DetectTouch();

            switch (touch)
            {
                case InputType.Down:
                    Debug.Log("swipe " + touch);
                    OnMove(Vector2Int.down);
                    break;
                case InputType.Left:
                    Debug.Log("swipe " + touch);
                    OnMove(Vector2Int.left);
                    break;
                case InputType.Right:
                    Debug.Log("swipe " + touch);
                    OnMove(Vector2Int.right);
                    break;
                case InputType.Release:
                    Debug.Log("click " + touch);

                    if (Input.GetTouch(0).position.x < Screen.width)
                        OnTurnCounterClockwise();
                    else
                        OnTurnClockwise();
                    break;
                default:
                    break;
            }
        }
    }
}