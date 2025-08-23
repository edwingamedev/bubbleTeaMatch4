using UnityEngine.UI;

namespace EdwinGameDev.BubbleTeaMatch4
{
    // todo: make this screen start the dev mode and use dev sessionController
    public class DevModeScreen : ScreenBehaviour
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
            sessionController.OnGameOver += GameOver;
            backButton.onClick.AddListener(ScreenManager.LoadPreviousScreen);
        }

        private void GameOver()
        {
            ScreenManager.LoadScreen(typeof(GameOverScreen));
        }

        public override void OnActivate()
        {
            gameObject.SetActive(true);

            sessionController.StartSelfAttackMode();
        }

        public override void OnDeactivate()
        {
            gameObject.SetActive(false);
        }
    }
}