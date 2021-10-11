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
        private GameBoard gameBoard;
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
            gameBoard.spawnEvilbubble.Enqueue(SpawnEvilBubble);
        }

        private void SpawnEvilBubble()
        {
            //Spawn new bubble set
            var x = evilSpawnPos % gameBoard.gridBehaviour.Grid.Size.x;
            var y = gameBoard.gridBehaviour.Grid.Size.y - 1;

            do
            {
                var evilBubble = gameBoard.bubbleSpawner.SpawnEvilBubble(new Vector2Int(x, y));
                gameBoard.gridBehaviour.Grid.AssignBubble(evilBubble, x, y);

                x = ++evilSpawnPos % gameBoard.gridBehaviour.Grid.Size.x;
            } while (gameBoard.gridBehaviour.Grid.IsOccupied(x, y));

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
            if (gameBoard != null)
            {
                ResetGame();
            }

            matchScenario.colorGenerator.Generate();

            var gridBehaviour = new GridBehaviour(gameSettings);
            var bubbleSpawner = new BubbleSpawner(gameSettings, boardOffset, bubblePooling, evilPooling);

            var inputController = new BubbleInputController(gridBehaviour.Grid,
                                                            inputProcessor,
                                                            () => matchScenario.scoreController.AddPoints(10));
            gameBoard = new GameBoard()
            {
                gameSettings = gameSettings,
                bubbleSpawner = bubbleSpawner,
                gridBehaviour = gridBehaviour,
                inputController = inputController,
                boardOffset = boardOffset
            };

            OnCombo += () => matchScenario.scoreController.AddPoints(50);

            stateMachineProvider = new GameStateProvider(gameBoard, OnCombo);
            gameStateMachine = stateMachineProvider.GetStateMachine(OnStart, OnGameOver);

            gameBoard.GameStarted = true;
        }

        private void ResetGame()
        {
            if (gameBoard != null)
            {
                gameBoard.GameStarted = false;
                enabled = true;
                gameBoard.bubbleSpawner.Reset();
                gameBoard.gridBehaviour.ResetGrid();
            }

            matchScenario.scoreController.ResetScore();
        }
    }
}