using UnityEngine;

namespace EdwinGameDev.BubbleTeaMatch4
{
    public class GameOverState : IState
    {
        public void OnEnter()
        {
            Debug.Log("GAME OVER");
        }

        public void OnExit() { }

        public void Tick() { }
    }
}