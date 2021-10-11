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

        public bool IsValidMovement(BubbleSet bubbleSet, Vector2Int direction)
        {
            Vector2Int mainNewPos = bubbleSet.Main.MovementController.GetPosition() + direction;
            Vector2Int subNewPos = bubbleSet.Sub.MovementController.GetPosition() + direction;

            return InBoundsHorizontally(mainNewPos) &&
                   InBoundsHorizontally(subNewPos) &&
                   ReachedBottom(bubbleSet.Main) &&
                   ReachedBottom(bubbleSet.Sub) &&
                   EmptyCell(mainNewPos) &&
                   EmptyCell(subNewPos);
        }

        private bool ReachedBottom(Bubble bubble)
        {
            return !grid.ReachedBottom(bubble);
        }

        private bool InBoundsHorizontally(Vector2Int position)
        {            
            return grid.InBoundsHorizontally(position);
        }

        private bool EmptyCell(Vector2Int position)
        {            
            return !grid.IsOccupied(position.x, position.y >= gridSize.y ? gridSize.y - 1 : position.y);
        }
    }
}