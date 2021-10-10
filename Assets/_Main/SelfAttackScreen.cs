using UnityEngine.UI;

namespace EdwinGameDev.BubbleTeaMatch4
{
    public class SelfAttackScreen : ScreenBehaviour
    {
        public GameSessionController sessionController;
        public Button backButton;

        private void Awake()
        {
            ScreenManager.AssignScreen(this);

            EventsAssignment();
        }


        // Update is called once per frame
        public void Update()
        {
            sessionController?.GameLoop();
        }

        private void EventsAssignment()
        {
            sessionController.OnGameOver += GameOver;
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
            sessionController.StartSelfAttackMode();
        }

        public override void OnDeactivate()
        {
            gameObject.SetActive(false);
        }
    }
}