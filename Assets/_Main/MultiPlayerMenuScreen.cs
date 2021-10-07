using UnityEngine.UI;

namespace EdwinGameDev.BubbleTeaMatch4
{
    public class MultiPlayerMenuScreen : ScreenBehaviour
    {
        public GameSessionController gameManager;
        public Button backButton;

        private void Awake()
        {
            ScreenManager.AssignScreen(this);

            EventsAssignment();
        }


        // Update is called once per frame
        public void Update()
        {
            gameManager?.GameLoop();
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
            gameManager.StartMultiplayer();
        }

        public override void OnDeactivate()
        {
            gameObject.SetActive(false);
        }
    }
}