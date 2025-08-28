using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace EdwinGameDev.BubbleTeaMatch4
{
    public class LinkingState : IState
    {
        private SessionVariables sessionVariables;

        public LinkingState(SessionVariables sessionVariables)
        {
            this.sessionVariables = sessionVariables;
        }

        public void OnEnter()
        {
            ResetLinkStatus();
            LinkBubbles();
        }

        private void ResetLinkStatus()
        {
            bool emptyRow;
            for (int y = 0; y < sessionVariables.gameSettings.GridSize.y; y++)
            {
                emptyRow = true;

                for (int x = 0; x < sessionVariables.gameSettings.GridSize.x; x++)
                {
                    if (!sessionVariables.gridBehaviour.Grid.IsOccupied(x, y))
                    {
                        continue;
                    }

                    emptyRow = false;
                    Bubble bubble = sessionVariables.gridBehaviour.Grid.GetBubble(x, y);
                    bubble.ConnectionController.Disconnect();

                    var newConnection = new List<Bubble>();
                    newConnection.Add(bubble);

                    bubble.ConnectionController.SetConnectionList(newConnection);
                }

                if (emptyRow)
                {
                    break;
                }
            }
        }

        private void LinkBubbles()
        {
            Grid grid = sessionVariables.gridBehaviour.Grid;
            Vector2Int gridSize = grid.Size;
            BubbleUnionFind unionFind = new();

            // Union neighboring bubbles of the same group
            for (int y = 0; y < gridSize.y; y++)
            {
                for (int x = 0; x < gridSize.x; x++)
                {
                    if (!grid.IsOccupied(x, y))
                    {
                        continue;
                    }

                    Bubble current = grid.GetBubble(x, y);

                    // Horizontal neighbor
                    HorizontalValidation(current, grid, unionFind, x, y);

                    // Vertical neighbor
                    VerticalValidation(current, grid, unionFind, x, y);
                }
            }

            // Build connection lists for each group
            Dictionary<Bubble, List<Bubble>> groups = new Dictionary<Bubble, List<Bubble>>();

            for (int y = 0; y < gridSize.y; y++)
            {
                for (int x = 0; x < gridSize.x; x++)
                {
                    if (!grid.IsOccupied(x, y))
                    {
                        continue;
                    }

                    Bubble bubble = grid.GetBubble(x, y);
                    Bubble root = unionFind.Find(bubble);

                    if (!groups.ContainsKey(root))
                    {
                        groups[root] = new List<Bubble>();
                    }

                    groups[root].Add(bubble);
                }
            }

            // Update each bubble’s ConnectionController with its group
            foreach (List<Bubble> group in groups.Values)
            {
                foreach (Bubble bubble in group)
                {
                    bubble.ConnectionController.SetConnectionList(group);
                    bubble.UpdateGraphics();
                }
            }
        }

        private void HorizontalValidation(Bubble current, Grid grid, BubbleUnionFind unionFind, int x, int y)
        {
            // Check inbounds
            if (x + 1 >= grid.Size.x ||
                !grid.IsOccupied(x + 1, y))
            {
                return;
            }

            Bubble right = grid.GetBubble(x + 1, y);
            if (current.bubbleGroup != right.bubbleGroup)
            {
                return;
            }

            unionFind.Union(current, right);
            BubbleConnector.ConnectHorizontal(current, right);
        }

        private void VerticalValidation(Bubble current, Grid grid, BubbleUnionFind unionFind, int x, int y)
        {
            // Check inbounds
            if (y + 1 >= grid.Size.y ||
                !grid.IsOccupied(x, y + 1))
            {
                return;
            }

            Bubble below = grid.GetBubble(x, y + 1);

            if (current.bubbleGroup != below.bubbleGroup)
            {
                return;
            }

            unionFind.Union(current, below);
            BubbleConnector.ConnectVertical(current, below);
        }

        public void OnExit()
        {
        }

        public void Tick()
        {
        }
    }
}