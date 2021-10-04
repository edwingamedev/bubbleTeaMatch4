namespace EdwinGameDev.BubbleTeaMatch4
{
    public class SpawnState : IState
    {
        private GameBoard gameVariables;

        public SpawnState(GameBoard gameVariables)
        {
            this.gameVariables = gameVariables;
        }

        public void OnEnter()
        {
            //Spawn new bubble set
            gameVariables.bubbleSpawner.SpawnNewBubbleSet();

            // Update controlled bubbles
            gameVariables.inputController.SetBubbles(gameVariables.bubbleSpawner.CurrentSet);
        }

        public void OnExit() { }

        public void Tick() { }
    }
}