using UnityEngine.UI;

namespace EdwinGameDev.BubbleTeaMatch4
{
    public class MenuScreen : ScreenBehaviour
    {
        public Button singleplayerButton;
        public Button multiplayerButton;
        public Button selfattackButton;
        public Button devModeButton;

        private void Awake()
        {
            ScreenManager.AssignScreen(this);
            ScreenManager.LoadScreen(typeof(MenuScreen));

            singleplayerButton.onClick.AddListener(StartSinglePlayer);
            multiplayerButton.onClick.AddListener(StartMultiplayer);
            selfattackButton.onClick.AddListener(StartSelfAttackMode);
            SetupDevMode();
        }

        private void SetupDevMode()
        {
#if !UNITY_EDITOR
            devModeButton.gameObject.SetActive(false);
#else
            devModeButton.gameObject.SetActive(true);
            devModeButton.onClick.AddListener(StartDevMode);
#endif
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

        private void StartDevMode()
        {
            ScreenManager.LoadScreen(typeof(DevModeScreen));
        }
    }
}