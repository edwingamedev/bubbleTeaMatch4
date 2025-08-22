using UnityEngine.UI;

namespace EdwinGameDev.BubbleTeaMatch4
{
    public class MenuScreen : ScreenBehaviour
    {        
        public Button singleplayerButton;
        public Button multiplayerButton;
        public Button selfattackButton;

        private void Awake()
        {
            ScreenManager.AssignScreen(this);
            ScreenManager.LoadScreen(typeof(MenuScreen));

            singleplayerButton.onClick.AddListener(StartSinglePlayer);
            multiplayerButton.onClick.AddListener(StartMultiplayer);
            selfattackButton.onClick.AddListener(StartSelfAttackMode); 
        }

        public override void OnActivate()
        {
            gameObject.SetActive(true);
        }

        public override void OnDeactivate()
        {
            gameObject.SetActive(false);
        }

        private void StartMultiplayer()
        {
            ScreenManager.LoadScreen(typeof(MultiPlayerMenuScreen));
        }

        private void StartSinglePlayer()
        {
            ScreenManager.LoadScreen(typeof(SinglePlayerMenuScreen));
        }

        private void StartSelfAttackMode()
        {
            ScreenManager.LoadScreen(typeof(SelfAttackScreen));
        }
    }
}