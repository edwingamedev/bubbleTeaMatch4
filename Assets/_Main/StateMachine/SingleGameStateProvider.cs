using System;

namespace EdwinGameDev.BubbleTeaMatch4
{
    public class SingleGameStateProvider : IStateMachineProvider
    {
        private StateMachine stateMachine;

        private GameBoard gameBoard;

        public SingleGameStateProvider(GameBoard gameBoard)
        {
            this.gameBoard = gameBoard;
        }

        public StateMachine GetStateMachine(Action OnStartGame, Action OnGameOver)
        {
            stateMachine = new StateMachine();

            // Create States
            var spawnState = new SpawnState(gameBoard); // playingState;
            var playingState = new PlayingState(gameBoard); // arrangeState; || gameOverState;
            var arrangeState = new ArrangeState(gameBoard); // linkingState;            
            var linkingState = new LinkingState(gameBoard); // comboState;
            var comboState = new ComboState(gameBoard); // spawnState || arrangeState;

            var gameOverState = new GameOverState(OnGameOver);
            var startState = new StartState(OnStartGame); // spawnState

            // Add Transitions

            // Start
            stateMachine.AddTransition(gameOverState, startState, PressedStart);
            // Game over
            stateMachine.AddTransition(playingState, gameOverState, CheckGameOver);
            // Spawn
            stateMachine.AddTransition(startState, spawnState, PressedStart);

            // Play
            stateMachine.AddTransition(spawnState, playingState, GoToNextState);
            // Arrange
            stateMachine.AddTransition(playingState, arrangeState, ReachedBottom);

            stateMachine.AddTransition(comboState, arrangeState, HasMatchesAndComboEnded);
            stateMachine.AddTransition(comboState, spawnState, FinishedCombo);

            // Link
            stateMachine.AddTransition(arrangeState, linkingState, BubblesRearranged);
            // Combo
            stateMachine.AddTransition(linkingState, comboState, GoToNextState);

            stateMachine.SetState(startState);

            return stateMachine;
        }

        // Setup conditions
        private bool PressedStart() => gameBoard.inputController.StartGame();
        private bool CheckGameOver() => gameBoard.gridBehaviour.OutOfBounds(gameBoard.bubbleSpawner.CurrentSet);
        private bool GoToNextState() => true;
        private bool BubblesRearranged() => gameBoard.BubbleRearranged;
        private bool ReachedBottom() => !CheckGameOver() && gameBoard.gridBehaviour.ReachedBottom(gameBoard.bubbleSpawner.CurrentSet);        
        private bool HasMatchesAndComboEnded() => gameBoard.HasMatches && !gameBoard.ComboStarted;
        private bool FinishedCombo() => !gameBoard.HasMatches && !gameBoard.ComboStarted;
    }
}