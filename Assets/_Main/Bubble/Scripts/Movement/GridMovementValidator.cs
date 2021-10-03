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
            gridSize = new Vector2Int(grid.cells.GetLength(0), grid.cells.GetLength(1));
        }

        public bool isValidDownMovement(BubbleSet bubbleSet)
        {
            return !grid.ReachedBottom(bubbleSet.Main) ||
                !grid.ReachedBottom(bubbleSet.Sub);
        }

        public bool IsValidHorizontalMovement(BubbleSet bubbleSet, Vector2Int direction)
        {
            Vector2Int mainNewPos = bubbleSet.Main.MovementController.GetPosition() + direction;
            Vector2Int subNewPos = bubbleSet.Sub.MovementController.GetPosition() + direction;

            return mainNewPos.x < gridSize.x &&
                   mainNewPos.x >= 0 &&
                   subNewPos.x < gridSize.x &&
                   subNewPos.x >= 0 &&
                   grid.cells[mainNewPos.x, mainNewPos.y >= gridSize.y ? gridSize.y - 1 : mainNewPos.y] == null &&
                   grid.cells[subNewPos.x, subNewPos.y >= gridSize.y ? gridSize.y - 1 : subNewPos.y] == null;
        }
    }
}