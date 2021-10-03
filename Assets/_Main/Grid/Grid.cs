using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EdwinGameDev.BubbleTeaMatch4
{
    public class Grid
    {
        public Bubble[,] cells;
        public Grid(Vector2Int size)
        {
            cells = new Bubble[size.x, size.y];
        }

        public bool ReachedBottom(Bubble bubble)
        {
            var position = bubble.MovementController.GetPosition();

            return position.y == 0 || cells[Mathf.CeilToInt(position.x), Mathf.CeilToInt(position.y - 1)] != null;
        }

        public void AssignBubble(Bubble bubble)
        {
            int x = bubble.MovementController.GetPosition().x;
            int y = bubble.MovementController.GetPosition().y;

            if (y < cells.GetLength(1))
            {
                if (cells[x, y] == null)
                {
                    cells[x, y] = bubble;
                    bubble.GraphicsController.DisableHighlight();
                }
                else
                {
                    Debug.Log("## Cell Occuped ## bubble error: " + bubble.name);
                }
            }
        }

        public void UnnassignBubble(Vector2Int position)
        {
            var bubble = cells[position.x, position.y];
            cells[position.x, position.y] = null;

            //TODO: Pooling
            Object.Destroy(bubble);
        }
    }
}