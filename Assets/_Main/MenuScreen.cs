﻿using UnityEngine;

namespace EdwinGameDev.BubbleTeaMatch4
{
    public class MenuScreen : ScreenBehaviour
    {
        private Canvas canvas;

        private void Awake()
        {
            canvas = GetComponentInChildren<Canvas>();

            ScreenManager.AssignScreen(this);

            ScreenManager.LoadScreen(typeof(MenuScreen));
        }

        public override void OnActivate()
        {
            gameObject.SetActive(true);
        }

        public override void OnDeactivate()
        {
            gameObject.SetActive(false);
        }

        public void SinglePlayer()
        {
            ScreenManager.LoadScreen(typeof(SinglePlayerMenuScreen));
        }
    }
}