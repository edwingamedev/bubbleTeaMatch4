using System;

namespace EdwinGameDev.BubbleTeaMatch4
{
    public class GameStateProvider : IStateMachineProvider
    {
        private StateMachine stateMachine;

        private GameBoard gameBoard;
        private Action OnCombo;

        public GameStateProvider(GameBoard gameBoard, Action OnCombo)
        {
            this.gameBoard = gameBoard;
            this.OnCombo = OnCombo;
        }

        public StateMachine GetStateMachine(Action OnStartGame, Action OnGameOver)
        {
            stateMachine = new StateMachine();

            // Create States
            var spawnState = new SpawnState(gameBoard); 
            var playingState = new PlayingState(gameBoard);
            var arrangeState = new ArrangeState(gameBoard);           
            var linkingState = new LinkingState(gameBoard);
            var comboState = new ComboState(gameBoard, OnCombo);
            var enemyAttack = new EnemyAttackState(gameBoard);

            var gameOverState = new GameOverState(OnGameOver);
            var startState = new StartState(OnStartGame);            

            // Add Transitions

            // Start
            stateMachine.AddTransition(gameOverState, startState, StartGame);
            // Game over
            stateMachine.AddTransition(playingState, gameOverState, CheckGameOver);
            // Spawn
            stateMachine.AddTransition(startState, spawnState, GoToNextState);

            // Play
            stateMachine.AddTransition(spawnState, playingState, GoToNextState);
            // Arrange
            stateMachine.AddTransition(playingState, arrangeState, ReachedBottom);

            stateMachine.AddTransition(comboState, arrangeState, HasMatchesAndComboEnded);
            stateMachine.AddTransition(comboState, enemyAttack, () => { return FinishedCombo() && !NoEvilBubble(); });
            stateMachine.AddTransition(comboState, spawnState, () => { return FinishedCombo() && NoEvilBubble(); });

            // Link
            stateMachine.AddTransition(arrangeState, linkingState, BubblesRearranged);
            // Combo
            stateMachine.AddTransition(linkingState, comboState, GoToNextState);

            // Enemy Attack
            stateMachine.AddTransition(enemyAttack, arrangeState, GoToNextState);

            stateMachine.SetState(startState);

            return stateMachine;
        }


        // Setup conditions
        private bool StartGame() => gameBoard.GameStarted;
        private bool CheckGameOver() => gameBoard.gridBehaviour.OutOfBounds(gameBoard.bubbleSpawner.CurrentSet);
        private bool NoEvilBubble() => gameBoard.spawnEvilbubble.Count == 0;
        private bool GoToNextState() => true;
        private bool BubblesRearranged() => gameBoard.BubbleRearranged;
        private bool ReachedBottom() => !CheckGameOver() && gameBoard.gridBehaviour.ReachedBottom(gameBoard.bubbleSpawner.CurrentSet);
        private bool HasMatchesAndComboEnded() => gameBoard.HasMatches && !gameBoard.ComboStarted;
        private bool FinishedCombo() => !gameBoard.HasMatches && !gameBoard.ComboStarted;
    }
}