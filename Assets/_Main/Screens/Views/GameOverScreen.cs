﻿using UnityEngine;
using UnityEngine.UI;

namespace EdwinGameDev.BubbleTeaMatch4
{
    public class GameOverScreen : ScreenBehaviour
    {        
        public Button startButton;
        public Button menuButton;

        private void Awake()
        {
            ScreenManager.AssignScreen(this);

            menuButton.onClick.AddListener(() => ScreenManager.LoadScreen(typeof(MenuScreen)));
            startButton.onClick.AddListener(() => ScreenManager.ReloadPreviousScreen());
        }

        public override void OnActivate()
        {
            Debug.Log("GameOver");

            gameObject.SetActive(true);
        }

        public override void OnDeactivate()
        {
            gameObject.SetActive(false);
        }
    }
}