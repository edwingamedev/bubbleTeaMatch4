using System;
using System.Threading.Tasks;

namespace EdwinGameDev.BubbleTeaMatch4
{
    public class ComboState : IState
    {
        private GameBoard gameVariables;
        private int popDelay = 500;

        public ComboState(GameBoard gameVariables)
        {
            this.gameVariables = gameVariables;
        }

        public void OnEnter()
        {
            gameVariables.ComboStarted = true;
                        
            _ = Combo();            
        }

        private async Task Combo()
        {
            if (ValidateMatches())
            {
                await PopAndFill(popDelay);

                gameVariables.ComboStarted = false;
                UnityEngine.Debug.Log("Combo MATCH");
            }
            else
            {
                gameVariables.ComboStarted = false;
                UnityEngine.Debug.Log("Combo NO MATCH");
            }
        }

        private bool ValidateMatches()
        {
            gameVariables.HasMatches = false;

            for (int y = 0; y < gameVariables.gameSettings.GridSize.y; y++)
            {
                bool emptyRow = true;
                for (int x = 0; x < gameVariables.gameSettings.GridSize.x; x++)
                {
                    if (gameVariables.gridBehaviour.Grid.cells[x, y] != null)
                    {
                        emptyRow = false;

                        if (gameVariables.gridBehaviour.Grid.cells[x, y].ConnectionController.GetConnectionList().Count >= 4)
                        {
                            gameVariables.HasMatches = true;
                            gameVariables.gridBehaviour.Grid.cells[x, y].ConnectionController.Matched = true;
                            gameVariables.gridBehaviour.Grid.cells[x, y].GraphicsController.PopAnimation();
                        }
                    }
                }
                if (emptyRow)
                    break;
            }

            //UpdateImage();
            return gameVariables.HasMatches;
        }

        private async Task PopAndFill(int taskDelay)
        {
            if (taskDelay != 0)
                await Task.Delay(taskDelay);

            for (int y = 0; y < gameVariables.gameSettings.GridSize.y; y++)
            {
                bool emptyRow = true;

                for (int x = 0; x < gameVariables.gameSettings.GridSize.x; x++)
                {
                    if (gameVariables.gridBehaviour.Grid.cells[x, y] != null)
                    {
                        emptyRow = false;

                        if (gameVariables.gridBehaviour.Grid.cells[x, y].ConnectionController.Matched)
                        {
                            UnityEngine.Object.Destroy(gameVariables.gridBehaviour.Grid.cells[x, y].gameObject);
                            gameVariables.gridBehaviour.Grid.cells[x, y] = null;

                            UnityEngine.Debug.Log("POPPED");

                            // ADD POINTS
                            //scoreController.AddPoints(10);
                        }                        
                    }                    
                }
                if (emptyRow)
                    break;
            }
        }

        public void OnExit() { }

        public void Tick() { }
    }
}