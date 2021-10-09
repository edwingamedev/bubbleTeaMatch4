using UnityEngine;

namespace EdwinGameDev.BubbleTeaMatch4
{
    public interface IBubbleBuilder
    {
        Bubble Generate(Vector2Int position);
        Bubble GenerateEvilBubble(Vector2Int position);
        void Reset();
    }
}