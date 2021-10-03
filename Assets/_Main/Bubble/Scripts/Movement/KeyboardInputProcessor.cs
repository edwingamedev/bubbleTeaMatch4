using UnityEngine;

namespace EdwinGameDev.BubbleTeaMatch4
{
    public class KeyboardInputProcessor : IInputProcessor
    {
        public bool StartGame()
        {
            return Input.GetKeyDown(KeyCode.Space);
        }

        public bool Down()
        {
            return Input.GetButton("Vertical") && Input.GetAxis("Vertical") < 0;
        }

        public bool Left()
        {
            return Input.GetButton("Horizontal") && Input.GetAxis("Horizontal") < 0;
        }

        public bool Right()
        {
            return Input.GetButton("Horizontal") && Input.GetAxis("Horizontal") > 0;
        }

        public bool TurnClockwise()
        {
            return Input.GetKeyDown(KeyCode.C);
        }

        public bool TurnCounterClockwise()
        {
            return Input.GetKeyDown(KeyCode.X);
        }
    }
}