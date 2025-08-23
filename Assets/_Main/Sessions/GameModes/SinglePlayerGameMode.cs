using System;

namespace EdwinGameDev.BubbleTeaMatch4
{
    public class SinglePlayerGameMode : IGameMode
    {
        private IGameSessionController gameController;
        public event Action OnGameOver;
        public event Action OnWin;

        public SinglePlayerGameMode(IGameSessionController gameController)
        {
            this.gameController = gameController;
        }

        public void StartMode()
        {
            EnableCamera();

            var session = gameController.GetOrCreateGameSession(0, new KeyboardInputProcessor());

            session.enabled = true;
            session.OnCombo += null;

            session.OnGameOver += OnGameOver;
            session.InitializeGame();
        }

        public void ExitMode()
        {
            var session = gameController.GetOrCreateGameSession(0, new KeyboardInputProcessor());
            session.enabled = false;
            
            session.OnGameOver -= OnGameOver;
        }

        private void EnableCamera()
        {
            gameController.EnableSoloCamera();
        }
    }
}