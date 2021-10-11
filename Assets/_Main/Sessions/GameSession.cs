using System;
using UnityEngine;

namespace EdwinGameDev.BubbleTeaMatch4
{
    [Serializable]
    public class GameSession
    {
        private GameSettings gameSettings;
        private Pooling bubblePooling;
        private Pooling evilPooling;
        private IInputProcessor inputProcessor;
        private SessionVariables sessionVariables;
        private MatchScenario matchScenario;
        private Vector2Int boardOffset;

        public Action OnStart;
        public Action OnGameOver;
        public Action OnCombo;

        public bool enabled;

        // GAME STATE MACHINE        
        private IStateMachineProvider stateMachineProvider;
        private StateMachine gameStateMachine;
        private int evilSpawnPos = 0;

        public void EnemyAttack()
        {
            sessionVariables.spawnEvilbubble.Enqueue(SpawnEvilBubble);
        }

        private void SpawnEvilBubble()
        {
            //Spawn new bubble set
            var x = evilSpawnPos % sessionVariables.gridBehaviour.Grid.Size.x;
            var y = sessionVariables.gridBehaviour.Grid.Size.y - 1;

            do
            {
                var evilBubble = sessionVariables.bubbleSpawner.SpawnEvilBubble(new Vector2Int(x, y));
                sessionVariables.gridBehaviour.Grid.AssignBubble(evilBubble, x, y);

                x = ++evilSpawnPos % sessionVariables.gridBehaviour.Grid.Size.x;
            } while (sessionVariables.gridBehaviour.Grid.IsOccupied(x, y));

        }

        public GameSession(GameSettings gameSettings, MatchScenario matchScenario, Vector2Int boardOffset, IInputProcessor inputProcessor, Pooling bubblePooling, Pooling evilPooling)
        {
            OnStart += ResetGame;

            this.gameSettings = gameSettings;
            this.matchScenario = matchScenario;
            this.inputProcessor = inputProcessor;
            this.boardOffset = boardOffset;
            this.bubblePooling = bubblePooling;
            this.evilPooling = evilPooling;

            matchScenario.transform.position = new Vector2(boardOffset.x, boardOffset.y);
        }

        public void Update()
        {
            gameStateMachine?.Execute();
        }

        public void InitializeSinglePlayer()
        {
            if (sessionVariables != null)
            {
                ResetGame();
            }

            matchScenario.colorGenerator.Generate();

            var gridBehaviour = new GridBehaviour(gameSettings);
            var bubbleSpawner = new BubbleSpawner(gameSettings, boardOffset, bubblePooling, evilPooling);

            var inputController = new BubbleInputController(gridBehaviour.Grid,
                                                            inputProcessor,
                                                            () => matchScenario.scoreController.AddPoints(10));
            sessionVariables = new SessionVariables()
            {
                gameSettings = gameSettings,
                bubbleSpawner = bubbleSpawner,
                gridBehaviour = gridBehaviour,
                inputController = inputController,
                boardOffset = boardOffset
            };

            OnCombo += () => matchScenario.scoreController.AddPoints(50);

            stateMachineProvider = new GameStateProvider(sessionVariables, OnCombo);
            gameStateMachine = stateMachineProvider.GetStateMachine(OnStart, OnGameOver);

            sessionVariables.GameStarted = true;
        }

        private void ResetGame()
        {
            if (sessionVariables != null)
            {
                sessionVariables.GameStarted = false;
                enabled = true;
                sessionVariables.bubbleSpawner.Reset();
                sessionVariables.gridBehaviour.ResetGrid();
            }

            matchScenario.scoreController.ResetScore();
        }
    }
}