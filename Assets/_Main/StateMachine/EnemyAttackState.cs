namespace EdwinGameDev.BubbleTeaMatch4
{
    public class EnemyAttackState : IState
    {
        private GameBoard gameVariables;

        public EnemyAttackState(GameBoard gameVariables)
        {
            this.gameVariables = gameVariables;
        }

        public void OnEnter()
        {
            SpawnEnemyEvilBubble();
        }

        private void SpawnEnemyEvilBubble()
        {
            while (gameVariables.spawnEvilbubble.Count > 0)
            {
                var spawn = gameVariables.spawnEvilbubble.Dequeue();
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