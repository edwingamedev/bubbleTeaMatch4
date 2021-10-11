using System;
using UnityEngine;

namespace EdwinGameDev.BubbleTeaMatch4
{
    public class BubbleInputController : IInputController
    {
        public IInputProcessor inputProcessor;
        private IMovementValidator movementValidator;
        private IOrientationController<Bubble> orientationController;

        private Grid grid;

        private BubbleSet bubbleSet;

        private Action OnMoveDown;

        public BubbleInputController(Grid grid, IInputProcessor inputProcessor, Action OnMoveDown)
        {
            this.grid = grid;
            this.OnMoveDown = OnMoveDown;

            this.inputProcessor = inputProcessor;


            inputProcessor.OnMove = (n) => ValidateAndMove(n);
            inputProcessor.OnTurnClockwise = TurnClockwise;
            inputProcessor.OnTurnCounterClockwise = TurnCounterClockwise;

            movementValidator = new GridMovementValidator(grid);
            orientationController = new BubbleOrientationController();
        }

        public void SetBubbles(BubbleSet bubbleSet)
        {
            this.bubbleSet = bubbleSet;
        }

        public void CheckInputs()
        {
            inputProcessor.CheckInputs();
        }

        private bool ValidateAndMove(Vector2Int moveDirection)
        {
            if (ValidateMovement(moveDirection))
            {
                // Move
                MoveBubbles(moveDirection);

                if (moveDirection.y < 0)
                {
                    // Movement Callback
                    OnMoveDown?.Invoke();
                }

                return true;
            }

            return false;
        }

        private bool ValidateMovement(Vector2Int moveDirection)
        {
            return movementValidator.IsValidMovement(bubbleSet, moveDirection);
        }

        public bool ValidateAndMoveDown()
        {
            return ValidateAndMove(Vector2Int.down);
        }

        private void MoveBubbles(Vector2Int moveDirection)
        {
            bubbleSet.Main?.MovementController.MoveDirection(moveDirection);
            bubbleSet.Sub?.MovementController.MoveDirection(moveDirection);
        }

        private void TurnClockwise()
        {
            var x = bubbleSet.Main.MovementController.GetPosition().x;
            var y = bubbleSet.Main.MovementController.GetPosition().y;
            var subPos = bubbleSet.Sub.MovementController.Orientation;

            if (bubbleSet.Sub == null)
                return;

            switch (subPos)
            {
                case Orientation.Bottom:
                    if (x - 1 >= 0 && grid.IsOccupied(x - 1, y))
                        break;

                    if (x != 0 || ValidateAndMove(Vector2Int.right))
                        orientationController.SetLeftOrientation(bubbleSet.Main, bubbleSet.Sub);

                    break;
                case Orientation.Top:
                    if (x + 1 < grid.Size.x && grid.IsOccupied(x + 1, y))
                        break;

                    if (x < grid.Size.x - 1 || ValidateAndMove(Vector2Int.left))
                        orientationController.SetRightOrientation(bubbleSet.Main, bubbleSet.Sub);

                    break;
                case Orientation.Left:
                    if (y + 1 < grid.Size.y && grid.IsOccupied(x, y + 1))
                        break;

                    orientationController.SetTopOrientation(bubbleSet.Main, bubbleSet.Sub);
                    break;
                case Orientation.Right:
                    if (y - 1 >= 0 && grid.IsOccupied(x, y - 1))
                        break;

                    orientationController.SetBottomOrientation(bubbleSet.Main, bubbleSet.Sub);
                    break;
                default:
                    break;
            }
        }

        private void TurnCounterClockwise()
        {
            var x = bubbleSet.Main.MovementController.GetPosition().x;
            var y = bubbleSet.Main.MovementController.GetPosition().y;
            var subPos = bubbleSet.Sub.MovementController.Orientation;

            switch (subPos)
            {
                case Orientation.Bottom:
                    if (x + 1 < grid.Size.x && grid.IsOccupied(x + 1, y))
                        break;

                    if (x < grid.Size.x - 1 || ValidateAndMove(Vector2Int.left))
                        orientationController.SetRightOrientation(bubbleSet.Main, bubbleSet.Sub);

                    break;
                case Orientation.Top:
                    if (x - 1 >= 0 && grid.IsOccupied(x - 1, y))
                        break;

                    if (x != 0 || ValidateAndMove(Vector2Int.right))
                        orientationController.SetLeftOrientation(bubbleSet.Main, bubbleSet.Sub);

                    break;
                case Orientation.Left:
                    if (y - 1 >= 0 && grid.IsOccupied(x, y - 1))
                        break;

                    orientationController.SetBottomOrientation(bubbleSet.Main, bubbleSet.Sub);
                    break;
                case Orientation.Right:
                    if (y + 1 < grid.Size.y && grid.IsOccupied(x, y + 1))
                        break;

                    orientationController.SetTopOrientation(bubbleSet.Main, bubbleSet.Sub);
                    break;
                default:
                    break;
            }
        }
    }
}