using System;
using UnityEngine;

namespace EdwinGameDev.BubbleTeaMatch4
{
    public class StartState : IState
    {
        private Action OnStartGame;

        public StartState(Action onStartGame)
        {
            OnStartGame = onStartGame;
        }

        public void OnEnter()
        {
            Debug.Log("START GAME");
        }

        public void OnExit()
        {
            OnStartGame?.Invoke();
        }

        public void Tick()
        {

        }
    }
}