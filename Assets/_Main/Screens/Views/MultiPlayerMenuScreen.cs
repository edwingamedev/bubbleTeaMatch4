using UnityEngine.UI;

namespace EdwinGameDev.BubbleTeaMatch4
{
    public class MultiPlayerMenuScreen : ScreenBehaviour
    {
        public GameSessionController sessionController;
        public Button backButton;

        private void Awake()
        {
            ScreenManager.AssignScreen(this);

            EventsAssignment();
        }

        public void Update()
        {
            sessionController?.GameLoop();
        }

        private void EventsAssignment()
        {
            sessionController.OnGameOver = GameOver;
            sessionController.OnWin = Win;
            backButton.onClick.AddListener(ScreenManager.LoadPreviousScreen);
        }

        private void Win()
        {
            ScreenManager.LoadScreen(typeof(WinScreen));
        }

        private void GameOver()
        {
            ScreenManager.LoadScreen(typeof(GameOverScreen));
        }

        public override void OnActivate()
        {
            gameObject.SetActive(true);
            
            sessionController.StartMultiplayer();
        }

        public override void OnDeactivate()
        {
            gameObject.SetActive(false);
        }
    }
}