using UnityEngine;

namespace EdwinGameDev.BubbleTeaMatch4
{
    public interface IMovementValidator
    {
        bool isValidDownMovement(Vector2Int main, Vector2Int sub);
        bool IsValidHorizontalMovement(Vector2Int main, Vector2Int sub, Vector2Int direction);
    }
}