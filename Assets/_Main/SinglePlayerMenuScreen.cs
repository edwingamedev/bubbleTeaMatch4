using UnityEngine;
using UnityEngine.UI;

namespace EdwinGameDev.BubbleTeaMatch4
{
    public class SinglePlayerMenuScreen : ScreenBehaviour
    {
        private Canvas canvas;
        public GameSessionController gameManager;
        //public Button startButton;
        public Button backButton;

        private void Awake()
        {
            canvas = GetComponentInChildren<Canvas>();
            ScreenManager.AssignScreen(this);

            EventsAssignment();
        }


        // Update is called once per frame
        public void Update()
        {
            gameManager?.Update();
        }

        private void EventsAssignment()
        {
            gameManager.OnGameOver += GameOver;
            backButton.onClick.AddListener(ScreenManager.LoadPreviousScreen);
        }

        public void GameOver()
        {
            ScreenManager.LoadScreen(typeof(GameOverScreen));
        }


        public override void OnActivate()
        {
            gameObject.SetActive(true);

            // Initialize Single Player
            gameManager.InitializeSinglePlayer();
        }

        public override void OnDeactivate()
        {
            gameObject.SetActive(false);
        }
    }
}