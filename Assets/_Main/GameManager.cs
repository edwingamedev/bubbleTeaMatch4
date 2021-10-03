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

        private GameBoard gameBoard;
        private bool paused;

        // STATE MACHINE        
        private IStateMachineProvider stateMachineProvider;
        private StateMachine gameStateMachine;

        void Awake()
        {
            Initialize();
        }

        private void Initialize()
        {
            var gridBehaviour = new GridBehaviour(gameSettings);
            var bubbleSpawner = new BubbleSpawner(gameSettings);

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
            gameStateMachine = stateMachineProvider.GetStateMachine();
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