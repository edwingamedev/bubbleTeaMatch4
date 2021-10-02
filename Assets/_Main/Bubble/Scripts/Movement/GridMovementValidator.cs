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

        public bool isValidDownMovement(Vector2Int main, Vector2Int sub)
        {
            return !grid.ReachedBottom(main) ||
                !grid.ReachedBottom(sub);
        }

        public bool IsValidHorizontalMovement(Vector2Int main, Vector2Int sub, Vector2Int direction)
        {
            Vector2Int mainNewPos = main + direction;
            Vector2Int subNewPos = sub + direction;

            return mainNewPos.x < gridSize.x &&
                   mainNewPos.x >= 0 &&
                   subNewPos.x < gridSize.x &&
                   subNewPos.x >= 0 &&
                   grid.cells[mainNewPos.x, mainNewPos.y >= gridSize.y ? gridSize.y - 1 : mainNewPos.y] == null &&
                   grid.cells[subNewPos.x, subNewPos.y >= gridSize.y ? gridSize.y - 1 : subNewPos.y] == null;
        }
    }
}