using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace EdwinGameDev.BubbleTeaMatch4
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private GameSettings gameSettings;
        [SerializeField] private ScoreController scoreController;

        [SerializeField] private Transform bubblePool;

        private GameBoard gameBoard;
        private bool paused;

        // GAME STATE MACHINE        
        private IStateMachineProvider stateMachineProvider;
        private StateMachine gameStateMachine;

        public Action OnStart;
        public Action OnGameOver;

        void Awake()
        {
            //InitializeSinglePlayer();
            OnGameOver += this.GameOver;
            OnStart += this.OnStartGame;
        }

        public void StartGame()
        {
            gameBoard.GameStarted = true;
        }

        public void InitializeSinglePlayer()
        {
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
        }

        private void OnStartGame()
        {
            gameBoard.GameStarted = false;
            gameBoard.bubbleSpawner.Reset();
            gameBoard.gridBehaviour.ResetGrid();
            scoreController.ResetScore();
        }

        private void GameOver()
        {
            Debug.Log("GameOver");
        }

        // Update is called once per frame
        void Update()
        {
            if (!paused)
            {
                gameStateMachine?.Execute();
            }

            // TODO: PAUSE BEHAVIOUR
            if (Input.GetKeyDown(KeyCode.P))
            {
                paused = !paused;
            }
        }
    }
}