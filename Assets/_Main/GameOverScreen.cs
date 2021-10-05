using UnityEngine;
using UnityEngine.UI;

namespace EdwinGameDev.BubbleTeaMatch4
{
    public class GameOverScreen : ScreenBehaviour
    {
        private Canvas canvas;
        public Button startButton;
        public Button menuButton;

        private void Awake()
        {
            canvas = GetComponentInChildren<Canvas>();

            ScreenManager.AssignScreen(this);

            menuButton.onClick.AddListener(() => ScreenManager.LoadScreen(typeof(MenuScreen)));
            startButton.onClick.AddListener(() => ScreenManager.LoadScreen(typeof(SinglePlayerMenuScreen)));
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