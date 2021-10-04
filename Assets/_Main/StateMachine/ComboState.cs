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
            }
            else
            {
                gameVariables.ComboStarted = false;
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
                    if (gameVariables.gridBehaviour.Grid.IsOccupied(x, y))
                    {
                        emptyRow = false;

                        if (gameVariables.gridBehaviour.Grid.GetBubble(x, y).ConnectionController.Matched)
                        {
                            gameVariables.HasMatches = true;

                            gameVariables.gridBehaviour.Grid.GetBubble(x, y).GraphicsController.PopAnimation();
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
                    if (gameVariables.gridBehaviour.Grid.IsOccupied(x, y))
                    {
                        emptyRow = false;

                        if (gameVariables.gridBehaviour.Grid.GetBubble(x, y).ConnectionController.Matched)
                        {
                            gameVariables.gridBehaviour.Grid.GetBubble(x, y).DisableObject();
                            gameVariables.gridBehaviour.Grid.UnnassignBubble(x, y);                                                     
                            
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