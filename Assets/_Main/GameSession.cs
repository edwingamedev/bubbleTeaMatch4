using System;
using UnityEngine;

namespace EdwinGameDev.BubbleTeaMatch4
{
    [System.Serializable]
    public class GameSession
    {
        private GameSettings gameSettings;
        private ScoreController scoreController;
        private Pooling pooling;
        private IInputProcessor inputProcessor;
        private GameBoard gameBoard;

        private Vector2 boardOffset;

        public Action OnStart;
        public Action OnGameOver;
        public Action OnCombo;

        // GAME STATE MACHINE        
        private IStateMachineProvider stateMachineProvider;
        private StateMachine gameStateMachine;

        public GameSession(GameSettings gameSettings, ScoreController scoreController, Vector2 boardOffset, IInputProcessor inputProcessor, Pooling pooling)
        {
            OnStart += ResetGame;

            this.gameSettings = gameSettings;
            this.scoreController = scoreController;
            this.inputProcessor = inputProcessor;

            this.pooling = pooling;
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

            var gridBehaviour = new GridBehaviour(gameSettings);
            var bubbleSpawner = new BubbleSpawner(gameSettings, pooling);

            var inputController = new BubbleInputController(gridBehaviour.Grid,
                                                            inputProcessor,
                                                            () => scoreController.AddPoints(10));
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

            scoreController.ResetScore();
        }
    }
}