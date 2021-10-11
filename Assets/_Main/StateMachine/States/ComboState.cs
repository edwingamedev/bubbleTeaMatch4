using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace EdwinGameDev.BubbleTeaMatch4
{
    public class ComboState : IState
    {
        private SessionVariables sessionVariables;
        private int popDelay = 500;
        private List<Vector2Int> matchesIndex;
        private Action OnCombo;

        public ComboState(SessionVariables sessionVariables, Action OnCombo)
        {
            matchesIndex = new List<Vector2Int>();
            this.sessionVariables = sessionVariables;
            this.OnCombo = OnCombo;
        }

        public void OnEnter()
        {
            sessionVariables.ComboStarted = true;

            _ = Combo();
        }

        private async Task Combo()
        {
            if (ValidateMatches())
            {
                await PopMatches(popDelay);
                               
                OnCombo?.Invoke();

                sessionVariables.ComboStarted = false;
            }
            else
            {
                sessionVariables.ComboStarted = false;
            }
        }

        private bool ValidateMatches()
        {
            matchesIndex.Clear();
            sessionVariables.HasMatches = false;

            for (int y = 0; y < sessionVariables.gameSettings.GridSize.y; y++)
            {
                bool emptyRow = true;
                for (int x = 0; x < sessionVariables.gameSettings.GridSize.x; x++)
                {
                    if (sessionVariables.gridBehaviour.Grid.IsOccupied(x, y))
                    {
                        emptyRow = false;

                        var bubble = sessionVariables.gridBehaviour.Grid.GetBubble(x, y);

                        if (bubble.ConnectionController.Matched())
                        {
                            matchesIndex.Add(new Vector2Int(x, y));
                            sessionVariables.HasMatches = true;

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
            return sessionVariables.HasMatches;
        }

        private void VerifyEvilBubble(int x, int y)
        {
            if (sessionVariables.gridBehaviour.Grid.InBounds(x,y) && 
                sessionVariables.gridBehaviour.Grid.IsOccupied(x, y))
            {
                Bubble newBubble = sessionVariables.gridBehaviour.Grid.GetBubble(x, y);
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
                sessionVariables.gridBehaviour.Grid.GetBubble(cellIndex.x, cellIndex.y).DisableObject();
                sessionVariables.gridBehaviour.Grid.UnnassignBubble(cellIndex.x, cellIndex.y);
            }
        }

        private async Task PopAndFill(int taskDelay)
        {
            if (taskDelay != 0)
                await Task.Delay(taskDelay);

            for (int y = 0; y < sessionVariables.gameSettings.GridSize.y; y++)
            {
                bool emptyRow = true;

                for (int x = 0; x < sessionVariables.gameSettings.GridSize.x; x++)
                {
                    if (sessionVariables.gridBehaviour.Grid.IsOccupied(x, y))
                    {
                        emptyRow = false;

                        if (sessionVariables.gridBehaviour.Grid.GetBubble(x, y).ConnectionController.Matched())
                        {
                            sessionVariables.gridBehaviour.Grid.GetBubble(x, y).DisableObject();
                            sessionVariables.gridBehaviour.Grid.UnnassignBubble(x, y);

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