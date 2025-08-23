using System;

namespace EdwinGameDev.BubbleTeaMatch4
{
    public class MultiplayerGameMode : IGameMode
    {
        private IGameSessionController gameController;
        public event Action OnGameOver;
        public event Action OnWin;
        GameSession session1;
        GameSession session2;

        public MultiplayerGameMode(IGameSessionController gameController)
        {
            this.gameController = gameController;
        }

        public void StartMode()
        {
            EnableCamera();

            session1 = gameController.GetOrCreateGameSession(0, new KeyboardInputProcessor());
            session2 = gameController.GetOrCreateGameSession(1, new AIInputProcessor());

            session1.enabled = true;
            session1.OnCombo += session2.EnemyAttack;
            session1.OnGameOver += OnGameOver;

            session2.enabled = true;
            session2.OnCombo += session1.EnemyAttack;
            session2.OnGameOver += OnWin;

            session1.InitializeGame();
            session2.InitializeGame();
        }

        public void ExitMode()
        {
            session1.enabled = false;
            session1.OnCombo -= session2.EnemyAttack;
            session1.OnGameOver -= OnGameOver;

            session2.enabled = false;
            session2.OnCombo -= session1.EnemyAttack;
            session2.OnGameOver -= OnWin;
        }

        private void EnableCamera()
        {
            gameController.EnableVSCamera();
        }
    }
}