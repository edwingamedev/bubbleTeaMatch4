using UnityEngine;

namespace EdwinGameDev.BubbleTeaMatch4
{
    public class BubbleMovementController
    {
        private GameSettings gameSettings;
        private Grid grid;
        private Bubble mainBubble;
        private Bubble subBubble;

        public BubbleMovementController(GameSettings gameSettings, Grid grid)
        {
            this.gameSettings = gameSettings;
            this.grid = grid;
        }

        public void SetBubbles(Bubble mainBubble, Bubble subBubble)
        {
            this.mainBubble = mainBubble;
            this.subBubble = subBubble;
        }

        public void MoveLeft()
        {
            var moveDirection = Vector2Int.left;

            if (IsValidHorizontalMovement(moveDirection))
            {
                mainBubble.MoveDirection(moveDirection);
                subBubble.MoveDirection(moveDirection);
            }
        }

        public void MoveRight()
        {
            var moveDirection = Vector2Int.right;

            if (IsValidHorizontalMovement(moveDirection))
            {
                mainBubble.MoveDirection(moveDirection);
                subBubble.MoveDirection(moveDirection);
            }
        }

        public void MoveDown()
        {
            var moveDirection = Vector2Int.down;

            if (isValidDownMovement())
            {
                mainBubble.MoveDirection(moveDirection);
                subBubble.MoveDirection(moveDirection);
            }
        }

        public void TurnClockwise()
        {

        }

        public void TurnCounterClockwise()
        {

        }

        private bool IsValidHorizontalMovement(Vector2Int direction)
        {
            Vector2Int mainBubblePos = mainBubble.GetPosition() + direction;
            Vector2Int subBubblePos = subBubble.GetPosition() + direction;

            return mainBubblePos.x < gameSettings.GridSize.x &&
                   mainBubblePos.x >= 0 &&
                   subBubblePos.x < gameSettings.GridSize.x &&
                   subBubblePos.x >= 0 &&
                   grid.cells[mainBubblePos.x, mainBubblePos.y >= gameSettings.GridSize.y ? gameSettings.GridSize.y - 1 : mainBubblePos.y] == null &&
                   grid.cells[subBubblePos.x, subBubblePos.y >= gameSettings.GridSize.y ? gameSettings.GridSize.y - 1 : subBubblePos.y] == null;
        }

        private bool isValidDownMovement()
        {
            Vector2Int mainBubblePos = mainBubble.GetPosition() + Vector2Int.down;
            Vector2Int subBubblePos = subBubble.GetPosition() + Vector2Int.down;

            return !grid.ReachedBottom(mainBubblePos) || !grid.ReachedBottom(subBubblePos);
        }
    }
}