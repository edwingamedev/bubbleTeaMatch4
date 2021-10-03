using UnityEngine;

namespace EdwinGameDev.BubbleTeaMatch4
{
    public interface IMovementValidator
    {
        bool isValidDownMovement(BubbleSet BubbleSet);
        bool IsValidHorizontalMovement(BubbleSet BubbleSet, Vector2Int direction);
    }
}