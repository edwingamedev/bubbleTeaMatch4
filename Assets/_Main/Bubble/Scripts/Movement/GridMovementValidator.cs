using UnityEngine;

namespace EdwinGameDev.BubbleTeaMatch4
{
    public class GridMovementValidator : IMovementValidator
    {
        private Grid grid;
        private Vector2Int gridSize;

        public GridMovementValidator(Grid grid)
        {
            this.grid = grid;
            gridSize = grid.Size;
        }

        private bool ReachedBottom(Bubble bubble)
        {
            return !grid.ReachedBottom(bubble);
        }

        public bool IsValidMovement(BubbleSet bubbleSet, Vector2Int direction)
        {
            Vector2Int mainNewPos = bubbleSet.Main.MovementController.GetPosition() + direction;
            Vector2Int subNewPos = bubbleSet.Sub.MovementController.GetPosition() + direction;

            return InBounds(mainNewPos) && 
                   InBounds(subNewPos) &&
                   ReachedBottom(bubbleSet.Main) &&
                   ReachedBottom(bubbleSet.Sub) &&
                   EmptyCell(mainNewPos) && 
                   EmptyCell(subNewPos);
        }

        private bool InBounds(Vector2Int position)
        {
            return position.x < gridSize.x && position.x >= 0;
        }

        private bool EmptyCell(Vector2Int position)
        {
            Debug.Log(position);
            return !grid.IsOccupied(position.x, position.y >= gridSize.y ? gridSize.y - 1 : position.y);
        }
    }
}