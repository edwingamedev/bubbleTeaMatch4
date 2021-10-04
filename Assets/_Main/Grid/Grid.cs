using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EdwinGameDev.BubbleTeaMatch4
{
    public class Grid
    {
        private Bubble[,] cells;

        public Vector2Int Size { get; }

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
                else
                {
                    //bubble.Disable();
                    Debug.Log("## Cell Occuped ## bubble error: " + bubble.name);
                }
            }
        }

        public void UnnassignBubble(int x, int y)
        {
            cells[x, y] = null;
        }
    }
}