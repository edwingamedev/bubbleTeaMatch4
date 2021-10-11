using System;
using UnityEngine;

namespace EdwinGameDev.BubbleTeaMatch4
{
    public class KeyboardInputProcessor : IInputProcessor
    {
        public Action OnTurnClockwise { get; set; }
        public Action OnTurnCounterClockwise { get; set; }
        public Action<Vector2Int> OnMove { get; set; }


        private float inputDelay = .1f;
        private float nextInput;
        private Vector2Int moveVector;

        public void CheckInputs()
        {
            // Movement
            if (Time.time > nextInput)
            {
                nextInput = Time.time + inputDelay;

                if (Left())
                    moveVector += Vector2Int.left;

                if (Right())
                    moveVector += Vector2Int.right;

                if (Down())
                    moveVector += Vector2Int.down;

                // Move
                if (moveVector != Vector2Int.zero)
                {
                    // Move vertical
                    OnMove(new Vector2Int(0, moveVector.y));

                    // Move horizontal
                    OnMove(new Vector2Int(moveVector.x, 0));
                }

                // Reset movement
                moveVector = Vector2Int.zero;
            }

            // Turn validation
            if (TurnClockwise())
            {
                OnTurnClockwise();
            }
            else if (TurnCounterClockwise())
            {
                OnTurnCounterClockwise();
            }
        }

        private bool Down()
        {
            return Input.GetButton("Vertical") && Input.GetAxis("Vertical") < 0;
        }

        private bool Left()
        {
            return Input.GetButton("Horizontal") && Input.GetAxis("Horizontal") < 0;
        }

        private bool Right()
        {
            return Input.GetButton("Horizontal") && Input.GetAxis("Horizontal") > 0;
        }

        private bool TurnClockwise()
        {
            return Input.GetKeyDown(KeyCode.C);
        }

        private bool TurnCounterClockwise()
        {
            return Input.GetKeyDown(KeyCode.X);
        }
    }
}