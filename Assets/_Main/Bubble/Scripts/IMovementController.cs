using UnityEngine;

namespace EdwinGameDev.BubbleTeaMatch4
{
    public interface IMovementController
    {
        void SetPosition(Vector2Int position);
        void MoveDirection(Vector2Int direction);
        Vector2Int GetPosition();
        Orientation Orientation { get; set; }

        void Reset();
    }
}