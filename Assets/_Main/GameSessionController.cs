using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace EdwinGameDev.BubbleTeaMatch4
{
    [System.Serializable]
    public class GameSessionController
    {
        [SerializeField] private GameSettings gameSettings;
        [SerializeField] private ScoreController scoreController;
        [SerializeField] private Transform bubblePool;

        private GameBoard gameBoard;

        // GAME STATE MACHINE        
        private IStateMachineProvider stateMachineProvider;
        private StateMachine gameStateMachine;

        public Action OnStart;
        public Action OnGameOver;

        public GameSessionController()
        {
            OnStart += ResetGame;
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
            var bubbleSpawner = new BubbleSpawner(gameSettings, bubblePool);

            var inputController = new BubbleInputController(gridBehaviour.Grid,
                                                        new KeyboardInputProcessor(),
                                                        () => scoreController.AddPoints(10));
            gameBoard = new GameBoard()
            {
                gameSettings = gameSettings,
                bubbleSpawner = bubbleSpawner,
                gridBehaviour = gridBehaviour,
                inputController = inputController
            };

            stateMachineProvider = new SingleGameStateProvider(gameBoard);
            gameStateMachine = stateMachineProvider.GetStateMachine(OnStart, OnGameOver);

            gameBoard.GameStarted = true;
        }

        private void ResetGame()
        {
            gameBoard.GameStarted = false;
            gameBoard.bubbleSpawner.Reset();
            gameBoard.gridBehaviour.ResetGrid();
            scoreController.ResetScore();
        }
    }
}