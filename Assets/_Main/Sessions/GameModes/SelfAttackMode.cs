using System;

namespace EdwinGameDev.BubbleTeaMatch4
{
    public class SelfAttackMode : IGameMode
    {
        private IGameSessionController gameController;
        public event Action OnGameOver;
        public event Action OnWin;
        GameSession session;

        public SelfAttackMode(IGameSessionController gameController)
        {
            this.gameController = gameController;
        }

        public void StartMode()
        {
            EnableCamera();

            session = gameController.GetOrCreateGameSession(0, new KeyboardInputProcessor());

            session.enabled = true;
            session.OnCombo += session.EnemyAttack;
            session.OnGameOver += OnGameOver;
            session.InitializeGame();
        }

        public void ExitMode()
        {
            session.enabled = false;

            session.OnCombo -= session.EnemyAttack;
            session.OnGameOver -= OnGameOver;
        }

        private void EnableCamera()
        {
            gameController.EnableSoloCamera();
        }
    }
}