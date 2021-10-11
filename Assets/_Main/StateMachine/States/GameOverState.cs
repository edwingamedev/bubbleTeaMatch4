using System;
using UnityEngine;

namespace EdwinGameDev.BubbleTeaMatch4
{
    public class GameOverState : IState
    {
        private Action OnGameOver;

        public GameOverState(Action onGameOver)
        {
            OnGameOver = onGameOver;
        }

        public void OnEnter()
        {
            OnGameOver?.Invoke();
        }

        public void OnExit() { }

        public void Tick() { }
    }
}