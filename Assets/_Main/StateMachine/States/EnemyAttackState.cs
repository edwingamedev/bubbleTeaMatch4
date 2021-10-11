namespace EdwinGameDev.BubbleTeaMatch4
{
    public class EnemyAttackState : IState
    {
        private SessionVariables sessionVariables;

        public EnemyAttackState(SessionVariables sessionVariables)
        {
            this.sessionVariables = sessionVariables;
        }

        public void OnEnter()
        {
            SpawnEnemyEvilBubble();
        }

        private void SpawnEnemyEvilBubble()
        {
            while (sessionVariables.spawnEvilbubble.Count > 0)
            {
                var spawn = sessionVariables.spawnEvilbubble.Dequeue();
                spawn?.Invoke();
            }
        }

        public void OnExit()
        {
            
        }

        public void Tick()
        {

        }
    }
}