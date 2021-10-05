using UnityEngine;
using UnityEngine.UI;

namespace EdwinGameDev.BubbleTeaMatch4
{
    public class SinglePlayerMenuScreen : ScreenBehaviour
    {
        private Canvas canvas;
        public GameManager gameManager;
        public Button startButton;

        private void Awake()
        {
            canvas = GetComponentInChildren<Canvas>();
            ScreenManager.AssignScreen(this);
            gameManager.OnStart += () => startButton.gameObject.SetActive(false);
            gameManager.OnGameOver += () => startButton.gameObject.SetActive(true);
            startButton.onClick.AddListener(StartGame);
        }

        public void StartGame()
        {
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