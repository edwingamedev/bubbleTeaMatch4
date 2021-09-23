using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid
{
    public Bubble[,] cells;
    public Grid(Vector2Int size)
    {
        cells = new Bubble[size.x, size.y];
    }

    public void AssignBubble(int x, int y, Bubble bubble)
    {
        cells[x, y] = bubble;
    }

    public void UnnassignBubble(int x, int y)
    {
        var bubble = cells[x, y];
        cells[x, y] = null;


        //TODO: Pooling
        Object.Destroy(bubble);
    }
}
