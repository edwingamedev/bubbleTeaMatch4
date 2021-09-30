using UnityEngine;

namespace EdwinGameDev.BubbleTeaMatch4
{
    public interface IBubbleBuilder
    {
        Bubble Generate(Vector2Int position);
    }
}