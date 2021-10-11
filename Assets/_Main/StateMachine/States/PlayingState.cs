using UnityEngine;

namespace EdwinGameDev.BubbleTeaMatch4
{
    public class PlayingState : IState
    {
        private SessionVariables sessionVariables;

        private float dropRate => sessionVariables.gameSettings.DropRate;
        private float nextDrop;

        public PlayingState(SessionVariables sessionVariables)
        {
            this.sessionVariables = sessionVariables;
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
            if (sessionVariables.gridBehaviour.ValidateBubbleMovement(sessionVariables.bubbleSpawner.CurrentSet))
            {
                sessionVariables.inputController.CheckInputs();
            }
        }

        private void BubbleDrop()
        {
            if (Time.time > nextDrop)
            {
                nextDrop = Time.time + dropRate;

                // Validate Bubble Drop
                if (sessionVariables.gridBehaviour.ValidateBubbleMovement(sessionVariables.bubbleSpawner.CurrentSet))
                {
                    // Move down
                    sessionVariables.inputController.ValidateAndMoveDown();
                }
            }
        }
    }
}