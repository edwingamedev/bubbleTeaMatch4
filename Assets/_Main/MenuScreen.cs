using UnityEngine;
using UnityEngine.UI;

namespace EdwinGameDev.BubbleTeaMatch4
{
    public class MenuScreen : ScreenBehaviour
    {        
        public Button singleplayerButton;
        public Button multiplayerButton;

        private void Awake()
        {
            ScreenManager.AssignScreen(this);
            ScreenManager.LoadScreen(typeof(MenuScreen));


            singleplayerButton.onClick.AddListener(StartSinglePlayer);
            multiplayerButton.onClick.AddListener(StartMultiplayer);
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
            ScreenManager.LoadScreen(typeof(SinglePlayerMenuScreen));
        }

        private void StartSinglePlayer()
        {
            ScreenManager.LoadScreen(typeof(SinglePlayerMenuScreen));
        }
    }
}