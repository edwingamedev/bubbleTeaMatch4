using UnityEngine;

namespace EdwinGameDev.BubbleTeaMatch4
{
    public class PlayingState : IState
    {
        private GameBoard gameVariables;

        private float dropRate = 1f;
        private float nextDrop;

        public PlayingState(GameBoard gameVariables)
        {
            this.gameVariables = gameVariables;
        }

        public void OnEnter() { }

        public void OnExit() { }

        public void Tick()
        {
            BubbleDrop();
            CheckInputs();
        }

        private void CheckInputs()
        {
            if (gameVariables.gridBehaviour.ValidateBubbleMovement(gameVariables.bubbleSpawner.CurrentSet))
            {
                gameVariables.inputController.CheckInputs();
            }
        }

        private void BubbleDrop()
        {
            if (Time.time > nextDrop)
            {
                nextDrop = Time.time + dropRate;

                // Validate Bubble Drop
                if (gameVariables.gridBehaviour.ValidateBubbleMovement(gameVariables.bubbleSpawner.CurrentSet))
                {
                    // Move down
                    gameVariables.inputController.ValidateAndMoveDown();
                }
            }
        }
    }
}