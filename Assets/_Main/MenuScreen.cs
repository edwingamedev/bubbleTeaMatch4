using UnityEngine;

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
            if (canvas)
                canvas.enabled = true;
        }

        public override void OnDeactivate()
        {
            if (canvas)
                canvas.enabled = false;
        }

        public void SinglePlayer()
        {
            ScreenManager.LoadScreen(typeof(SinglePlayerMenuScreen));
        }
    }
}