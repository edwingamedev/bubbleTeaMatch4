using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EdwinGameDev.BubbleTeaMatch4
{
    public class Grid
    {
        private Bubble[,] cells = new Bubble[0,0];

        public Vector2Int Size { get; }

        public void ResetGrid()
        {
            cells = new Bubble[Size.x, Size.y];
        }

        public Grid(Vector2Int size)
        {
            cells = new Bubble[size.x, size.y];
            this.Size = size;
        }

        public Bubble GetBubble(int x, int y)
        {
            return cells[x, y];
        }

        public bool IsOccupied(int x, int y)
        {
            return cells[x, y] != null;
        }

        public bool ReachedBottom(Bubble bubble)
        {
            var position = bubble.MovementController.GetPosition();

            return position.y == 0 || IsOccupied(Mathf.CeilToInt(position.x), Mathf.CeilToInt(position.y-1));
        }

        public void AssignBubble(Bubble bubble, int x, int y)
        {
            if (y < cells.GetLength(1))
            {
                if (cells[x, y] == null)
                {
                    cells[x, y] = bubble;
                    bubble.GraphicsController.DisableHighlight();
                }
            }
        }

        public void UnnassignBubble(int x, int y)
        {
            cells[x, y] = null;
        }
    }
}