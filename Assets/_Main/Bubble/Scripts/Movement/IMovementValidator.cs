using UnityEngine;

namespace EdwinGameDev.BubbleTeaMatch4
{
    public interface IMovementValidator
    {
        bool IsValidMovement(BubbleSet BubbleSet, Vector2Int direction);
    }
}