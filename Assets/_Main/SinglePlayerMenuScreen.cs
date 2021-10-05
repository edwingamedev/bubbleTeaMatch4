using UnityEngine;
using UnityEngine.UI;

namespace EdwinGameDev.BubbleTeaMatch4
{
    public class SinglePlayerMenuScreen : ScreenBehaviour
    {
        private Canvas canvas;
        public GameManager gameManager;
        public Button startButton;
        public Button backButton;

        private void Awake()
        {
            canvas = GetComponentInChildren<Canvas>();
            ScreenManager.AssignScreen(this);

            EventsAssignment();
        }

        private void EventsAssignment()
        {
            //gameManager.OnStart += () => startButton.gameObject.SetActive(false);
            gameManager.OnGameOver += () => startButton.gameObject.SetActive(true);

            startButton.onClick.AddListener(StartGame);
            backButton.onClick.AddListener(ScreenManager.LoadPreviousScreen);
        }

        public void StartGame()
        {
            startButton.gameObject.SetActive(false);
            gameManager.StartGame();
        }

        public override void OnActivate()
        {
            if (canvas)
                canvas.enabled = true;

            // Initialize Single Player
            gameManager.InitializeSinglePlayer();
        }

        public override void OnDeactivate()
        {
            if (canvas)
                canvas.enabled = false;
        }
    }
}