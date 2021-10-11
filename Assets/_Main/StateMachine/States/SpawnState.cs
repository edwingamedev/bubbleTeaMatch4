namespace EdwinGameDev.BubbleTeaMatch4
{
    public class SpawnState : IState
    {
        private SessionVariables sessionVariables;

        public SpawnState(SessionVariables sessionVariables)
        {
            this.sessionVariables = sessionVariables;
        }

        public void OnEnter()
        {
            //Spawn new bubble set
            sessionVariables.bubbleSpawner.SpawnNewBubbleSet();

            // Update controlled bubbles
            sessionVariables.inputController.SetBubbles(sessionVariables.bubbleSpawner.CurrentSet);
        }

        public void OnExit() { }

        public void Tick() { }
    }
}