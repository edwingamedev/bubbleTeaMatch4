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

        public bool ReachedBottom(Vector2 position)
        {
            return position.y <= 0 || cells[Mathf.CeilToInt(position.x), Mathf.CeilToInt(position.y - 1)] != null;
        }

        public void AssignBubble(Bubble bubble)
        {
            if (bubble.GetPosition().y < cells.GetLength(1))
                cells[bubble.GetPosition().x, bubble.GetPosition().y] = bubble;
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