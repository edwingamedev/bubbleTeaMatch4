using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EdwinGameDev.BubbleTeaMatch4
{
    public class ComboState : IState
    {
        private SessionVariables sessionVariables;
        private readonly float popDelay = 0.5f;
        private readonly HashSet<Vector2Int> matchesIndexSet = new();
        private event Action OnCombo;

        public ComboState(SessionVariables sessionVariables, Action OnCombo)
        {
            this.sessionVariables = sessionVariables;
            this.OnCombo = OnCombo;
        }

        public void OnEnter()
        {
            sessionVariables.ComboStarted = true;
            CoroutineRunner.Instance.Run(Combo());
        }

        private IEnumerator Combo()
        {
            if (ValidateMatches())
            {
                yield return PopMatches();
                OnCombo?.Invoke();
            }

            sessionVariables.ComboStarted = false;
        }

        // Todo: mark bubbles above popped as dirty so we can iterate over then on ArrangeState 
        private bool ValidateMatches()
        {
            matchesIndexSet.Clear();
            sessionVariables.HasMatches = false;

            Grid grid = sessionVariables.gridBehaviour.Grid;
            Vector2Int gridSize = sessionVariables.gameSettings.GridSize;

            for (int y = 0; y < gridSize.y; y++)
            {
                bool emptyRow = true;

                for (int x = 0; x < gridSize.x; x++)
                {
                    if (!grid.IsOccupied(x, y))
                    {
                        continue;
                    }

                    emptyRow = false;

                    Bubble bubble = grid.GetBubble(x, y);

                    if (!bubble.ConnectionController.Matched())
                    {
                        continue;
                    }

                    Vector2Int pos = new(x, y);
                    AddMatch(pos, bubble);

                    // Check neighbors
                    TryAddEvilBubble(x + 1, y);
                    TryAddEvilBubble(x - 1, y);
                    TryAddEvilBubble(x, y + 1);
                    TryAddEvilBubble(x, y - 1);
                }

                if (emptyRow)
                {
                    break;
                }
            }

            return sessionVariables.HasMatches;
        }

        private void AddMatch(Vector2Int pos, Bubble bubble)
        {
            if (!matchesIndexSet.Add(pos))
            {
                return;
            }

            sessionVariables.HasMatches = true;
            bubble.GraphicsController.PopAnimation();
        }

        private void TryAddEvilBubble(int x, int y)
        {
            Grid grid = sessionVariables.gridBehaviour.Grid;
            if (!grid.InBounds(x, y) || !grid.IsOccupied(x, y))
            {
                return;
            }

            Bubble bubble = grid.GetBubble(x, y);
            if (bubble.bubbleGroup != -1)
            {
                return;
            }

            AddMatch(new Vector2Int(x, y), bubble);
        }

        private IEnumerator PopMatches()
        {
            yield return new WaitForSeconds(popDelay);

            Grid grid = sessionVariables.gridBehaviour.Grid;

            foreach (Vector2Int cellIndex in matchesIndexSet)
            {
                Bubble bubble = grid.GetBubble(cellIndex.x, cellIndex.y);
                bubble.DisableObject();
                grid.UnassignBubble(cellIndex.x, cellIndex.y);
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