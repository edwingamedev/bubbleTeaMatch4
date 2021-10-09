using System;
using UnityEngine;

namespace EdwinGameDev.BubbleTeaMatch4
{
    [System.Serializable]
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

        // GAME STATE MACHINE        
        private IStateMachineProvider stateMachineProvider;
        private StateMachine gameStateMachine;

        public GameSession(GameSettings gameSettings, MatchScenario matchScenario, Vector2Int boardOffset, IInputProcessor inputProcessor, Pooling pooling)
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

            stateMachineProvider = new SingleGameStateProvider(gameBoard);
            gameStateMachine = stateMachineProvider.GetStateMachine(OnStart, OnGameOver);

            gameBoard.GameStarted = true;
        }

        private void ResetGame()
        {
            if (gameBoard != null)
            {
                gameBoard.GameStarted = false;
                gameBoard.bubbleSpawner.Reset();
                gameBoard.gridBehaviour.ResetGrid();
            }

            matchScenario.scoreController.ResetScore();
        }
    }
}