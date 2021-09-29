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

        public bool MoveLeft()
        {
            var moveDirection = Vector2Int.left;

            if (IsValidHorizontalMovement(moveDirection))
            {
                mainBubble?.MoveDirection(moveDirection);
                subBubble?.MoveDirection(moveDirection);
                return true;
            }

            return false;
        }

        public bool MoveRight()
        {
            var moveDirection = Vector2Int.right;

            if (IsValidHorizontalMovement(moveDirection))
            {
                mainBubble?.MoveDirection(moveDirection);
                subBubble?.MoveDirection(moveDirection);

                return true;
            }

            return false;
        }

        public bool MoveDown()
        {
            var moveDirection = Vector2Int.down;

            if (isValidDownMovement())
            {
                mainBubble?.MoveDirection(moveDirection);
                subBubble?.MoveDirection(moveDirection);
                return true;
            }

            return false;
        }

        private void SetLeftOrientation()
        {
            subBubble.SetPosition(new Vector2Int(mainBubble.GetPosition().x - 1, mainBubble.GetPosition().y));

            // Set New Orientation
            subBubble.Orientation = Orientation.Left;
        }

        private void SetRightOrientation()
        {
            subBubble.SetPosition(new Vector2Int(mainBubble.GetPosition().x + 1, mainBubble.GetPosition().y));

            // Set New Orientation
            subBubble.Orientation = Orientation.Right;
        }

        private void SetTopOrientation()
        {
            subBubble.SetPosition(new Vector2Int(mainBubble.GetPosition().x, mainBubble.GetPosition().y + 1));

            // Set New Orientation
            subBubble.Orientation = Orientation.Top;
        }

        private void SetBottomOrientation()
        {
            subBubble.SetPosition(new Vector2Int(mainBubble.GetPosition().x, mainBubble.GetPosition().y - 1));

            // Set New Orientation
            subBubble.Orientation = Orientation.Bottom;
        }

        public void TurnClockwise()
        {
            if (subBubble == null)
                return;

            Orientation subPos = subBubble.Orientation;

            int x = mainBubble.GetPosition().x;
            int y = mainBubble.GetPosition().y;

            switch (subPos)
            {
                case Orientation.Bottom:
                    if (x - 1 >= 0 && grid.cells[x - 1, y] != null)
                        break;

                    if (x != 0 || MoveRight())
                        SetLeftOrientation();

                    break;
                case Orientation.Top:
                    if (x + 1 < gameSettings.GridSize.x && grid.cells[x + 1, y] != null)
                        break;

                    if (x < gameSettings.GridSize.x - 1 || MoveLeft())
                        SetRightOrientation();    
                    
                    break;
                case Orientation.Left:
                    if (y + 1 < gameSettings.GridSize.y && grid.cells[x, y + 1] != null)
                        break;

                    SetTopOrientation();

                    break;
                case Orientation.Right:
                    if (y - 1 >= 0 && grid.cells[x, y - 1] != null)
                        break;

                    SetBottomOrientation();

                    break;
                default:
                    break;
            }
        }

        public void TurnCounterClockwise()
        {
            if (subBubble == null)
                return;

            Orientation subPos = subBubble.Orientation;
            int x = mainBubble.GetPosition().x;
            int y = mainBubble.GetPosition().y;

            switch (subPos)
            {
                case Orientation.Bottom:
                    if (x + 1 < gameSettings.GridSize.x && grid.cells[x + 1, y] != null)
                        break;

                    if (x < gameSettings.GridSize.x - 1 || MoveLeft())
                        SetRightOrientation();

                    break;
                case Orientation.Top:
                    if (x - 1 >= 0 && grid.cells[x - 1, y] != null)
                        break;

                    if (x != 0 || MoveRight())
                        SetLeftOrientation();

                    break;
                case Orientation.Left:
                    if (y - 1 >= 0 && grid.cells[x, y - 1] != null)
                        break;

                    SetBottomOrientation();
                    break;
                case Orientation.Right:
                    if (y + 1 < gameSettings.GridSize.y && grid.cells[x, y + 1] != null)
                        break;

                    SetTopOrientation();

                    break;
                default:
                    break;
            }
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
            Vector2Int mainBubblePos = mainBubble.GetPosition();
            Vector2Int subBubblePos = subBubble.GetPosition();

            return !grid.ReachedBottom(mainBubblePos) ||
                !grid.ReachedBottom(subBubblePos);
        }
    }
}