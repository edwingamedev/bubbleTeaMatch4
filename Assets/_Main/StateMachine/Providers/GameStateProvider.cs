using System;

namespace EdwinGameDev.BubbleTeaMatch4
{
    public class GameStateProvider : IStateMachineProvider
    {
        private StateMachine stateMachine;

        private SessionVariables sessionVariables;
        private Action OnCombo;

        public GameStateProvider(SessionVariables sessionVariables, Action OnCombo)
        {
            this.sessionVariables = sessionVariables;
            this.OnCombo = OnCombo;
        }

        public StateMachine GetStateMachine(Action OnStartGame, Action OnGameOver)
        {
            stateMachine = new StateMachine();

            // Create States
            var spawnState = new SpawnState(sessionVariables); 
            var playingState = new PlayingState(sessionVariables);
            var arrangeState = new ArrangeState(sessionVariables);           
            var linkingState = new LinkingState(sessionVariables);
            var comboState = new ComboState(sessionVariables, OnCombo);
            var enemyAttack = new EnemyAttackState(sessionVariables);

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
        private bool StartGame() => sessionVariables.GameStarted;
        private bool CheckGameOver() => sessionVariables.gridBehaviour.OutOfBounds(sessionVariables.bubbleSpawner.CurrentSet);
        private bool NoEvilBubble() => sessionVariables.spawnEvilbubble.Count == 0;
        private bool GoToNextState() => true;
        private bool BubblesRearranged() => sessionVariables.BubbleRearranged;
        private bool ReachedBottom() => !CheckGameOver() && sessionVariables.gridBehaviour.ReachedBottom(sessionVariables.bubbleSpawner.CurrentSet);
        private bool HasMatchesAndComboEnded() => sessionVariables.HasMatches && !sessionVariables.ComboStarted;
        private bool FinishedCombo() => !sessionVariables.HasMatches && !sessionVariables.ComboStarted;
    }
}