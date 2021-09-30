using UnityEngine;

namespace EdwinGameDev.BubbleTeaMatch4
{
    public class KeyboardInputProcessor : IInputProcessor
    {
        public bool MoveDown()
        {
            throw new System.NotImplementedException();
        }

        public bool MoveLeft()
        {
            throw new System.NotImplementedException();
        }

        public bool MoveRight()
        {
            throw new System.NotImplementedException();
        }

        public void TurnClockwise()
        {
            throw new System.NotImplementedException();
        }

        public void TurnCounterClockwise()
        {
            throw new System.NotImplementedException();
        }
    }

    public class InputController : MonoBehaviour
    {
        public IInputProcessor inputProcessor;
        private GameSettings gameSettings;
        private Grid grid;
        private Bubble mainBubble;
        private Bubble subBubble;

        public InputController(GameSettings gameSettings, Grid grid)
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
                mainBubble?.MovementController.MoveDirection(moveDirection);
                subBubble?.MovementController.MoveDirection(moveDirection);
                return true;
            }

            return false;
        }

        public bool MoveRight()
        {
            var moveDirection = Vector2Int.right;

            if (IsValidHorizontalMovement(moveDirection))
            {
                mainBubble?.MovementController.MoveDirection(moveDirection);
                subBubble?.MovementController.MoveDirection(moveDirection);

                return true;
            }

            return false;
        }

        public bool MoveDown()
        {
            var moveDirection = Vector2Int.down;

            if (isValidDownMovement())
            {
                mainBubble?.MovementController.MoveDirection(moveDirection);
                subBubble?.MovementController.MoveDirection(moveDirection);
                return true;
            }

            return false;
        }

        public void TurnClockwise()
        {
            if (subBubble == null)
                return;

            Orientation subPos = subBubble.MovementController.Orientation;

            int x = mainBubble.MovementController.GetPosition().x;
            int y = mainBubble.MovementController.GetPosition().y;

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

            Orientation subPos = subBubble.MovementController.Orientation;
            int x = mainBubble.MovementController.GetPosition().x;
            int y = mainBubble.MovementController.GetPosition().y;

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

        private void SetLeftOrientation()
        {
            subBubble.MovementController.SetPosition(new Vector2Int(mainBubble.MovementController.GetPosition().x - 1, mainBubble.MovementController.GetPosition().y));

            // Set New Orientation
            subBubble.MovementController.Orientation = Orientation.Left;
        }

        private void SetRightOrientation()
        {
            subBubble.MovementController.SetPosition(new Vector2Int(mainBubble.MovementController.GetPosition().x + 1, mainBubble.MovementController.GetPosition().y));

            // Set New Orientation
            subBubble.MovementController.Orientation = Orientation.Right;
        }

        private void SetTopOrientation()
        {
            subBubble.MovementController.SetPosition(new Vector2Int(mainBubble.MovementController.GetPosition().x, mainBubble.MovementController.GetPosition().y + 1));

            // Set New Orientation
            subBubble.MovementController.Orientation = Orientation.Top;
        }

        private void SetBottomOrientation()
        {
            subBubble.MovementController.SetPosition(new Vector2Int(mainBubble.MovementController.GetPosition().x, mainBubble.MovementController.GetPosition().y - 1));

            // Set New Orientation
            subBubble.MovementController.Orientation = Orientation.Bottom;
        }


        private bool IsValidHorizontalMovement(Vector2Int direction)
        {
            Vector2Int mainBubblePos = mainBubble.MovementController.GetPosition() + direction;
            Vector2Int subBubblePos = subBubble.MovementController.GetPosition() + direction;

            return mainBubblePos.x < gameSettings.GridSize.x &&
                   mainBubblePos.x >= 0 &&
                   subBubblePos.x < gameSettings.GridSize.x &&
                   subBubblePos.x >= 0 &&
                   grid.cells[mainBubblePos.x, mainBubblePos.y >= gameSettings.GridSize.y ? gameSettings.GridSize.y - 1 : mainBubblePos.y] == null &&
                   grid.cells[subBubblePos.x, subBubblePos.y >= gameSettings.GridSize.y ? gameSettings.GridSize.y - 1 : subBubblePos.y] == null;
        }

        private bool isValidDownMovement()
        {
            Vector2Int mainBubblePos = mainBubble.MovementController.GetPosition();
            Vector2Int subBubblePos = subBubble.MovementController.GetPosition();

            return !grid.ReachedBottom(mainBubblePos) ||
                !grid.ReachedBottom(subBubblePos);
        }
    }
}