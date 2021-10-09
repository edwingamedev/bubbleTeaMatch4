using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace EdwinGameDev.BubbleTeaMatch4
{
    public class ComboState : IState
    {
        private GameBoard gameVariables;
        private int popDelay = 500;
        private List<Vector2Int> matchesIndex;
        private Action OnCombo;

        public ComboState(GameBoard gameVariables, Action OnCombo)
        {
            matchesIndex = new List<Vector2Int>();
            this.gameVariables = gameVariables;
            this.OnCombo = OnCombo;
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
                await PopMatches(popDelay);
                               
                OnCombo?.Invoke();

                gameVariables.ComboStarted = false;
            }
            else
            {
                gameVariables.ComboStarted = false;
            }
        }

        private bool ValidateMatches()
        {
            matchesIndex.Clear();
            gameVariables.HasMatches = false;

            for (int y = 0; y < gameVariables.gameSettings.GridSize.y; y++)
            {
                bool emptyRow = true;
                for (int x = 0; x < gameVariables.gameSettings.GridSize.x; x++)
                {
                    if (gameVariables.gridBehaviour.Grid.IsOccupied(x, y))
                    {
                        emptyRow = false;

                        var bubble = gameVariables.gridBehaviour.Grid.GetBubble(x, y);

                        if (bubble.ConnectionController.Matched())
                        {
                            matchesIndex.Add(new Vector2Int(x, y));
                            gameVariables.HasMatches = true;

                            bubble.GraphicsController.PopAnimation();

                            VerifyEvilBubble(x + 1, y);
                            VerifyEvilBubble(x - 1, y);
                            VerifyEvilBubble(x, y + 1);
                            VerifyEvilBubble(x, y - 1);
                        }
                    }
                }
                if (emptyRow)
                    break;
            }

            //UpdateImage();
            return gameVariables.HasMatches;
        }

        private void VerifyEvilBubble(int x, int y)
        {
            if (gameVariables.gridBehaviour.Grid.InBounds(x,y) && 
                gameVariables.gridBehaviour.Grid.IsOccupied(x, y))
            {
                Bubble newBubble = gameVariables.gridBehaviour.Grid.GetBubble(x, y);
                if (newBubble.bubbleGroup == -1)
                {
                    var pos = new Vector2Int(x, y);

                    if (!matchesIndex.Contains(pos))
                    {
                        matchesIndex.Add(pos);
                        newBubble.GraphicsController.PopAnimation();
                    }                    
                }
            }
        }

        private async Task PopMatches(int taskDelay)
        {
            if (taskDelay != 0)
                await Task.Delay(taskDelay);

            foreach (var cellIndex in matchesIndex)
            {
                gameVariables.gridBehaviour.Grid.GetBubble(cellIndex.x, cellIndex.y).DisableObject();
                gameVariables.gridBehaviour.Grid.UnnassignBubble(cellIndex.x, cellIndex.y);
            }
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

                        if (gameVariables.gridBehaviour.Grid.GetBubble(x, y).ConnectionController.Matched())
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