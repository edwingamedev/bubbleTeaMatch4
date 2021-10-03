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
        private Vector2Int gridSize;

        private BubbleSet bubbleSet;

        private float inputDelay = .1f;
        private float nextInput;

        private Action OnMoveDown;

        public BubbleInputController(Grid grid, IInputProcessor inputProcessor, Action OnMoveDown)
        {
            this.grid = grid;
            gridSize = new Vector2Int(grid.cells.GetLength(0), grid.cells.GetLength(1));

            this.OnMoveDown = OnMoveDown;

            this.inputProcessor = inputProcessor;
            movementValidator = new GridMovementValidator(grid);
            orientationController = new BubbleOrientationController();
        }

        public void SetBubbles(BubbleSet bubbleSet)
        {
            this.bubbleSet = bubbleSet;
        }

        public void CheckInputs()
        {
            if (Time.time > nextInput)
            {
                nextInput = Time.time + inputDelay;

                if (inputProcessor.Left())
                {
                    ValidateAndMoveLeft();
                }

                if (inputProcessor.Right())
                {
                    ValidateAndMoveRight();
                }

                if (inputProcessor.Down())
                {
                    if (ValidateAndMoveDown())
                    {
                        // Movement Callback
                        OnMoveDown?.Invoke();
                    }
                }
            }

            if (inputProcessor.TurnClockwise())
            {
                TurnClockwise(bubbleSet.Main.MovementController.GetPosition().x,
                              bubbleSet.Main.MovementController.GetPosition().y,
                              bubbleSet.Sub.MovementController.Orientation);
            }

            if (inputProcessor.TurnCounterClockwise())
            {
                TurnCounterClockwise(bubbleSet.Main.MovementController.GetPosition().x,
                                    bubbleSet.Main.MovementController.GetPosition().y,
                                    bubbleSet.Sub.MovementController.Orientation);
            }
        }

        private bool ValidateAndMoveLeft()
        {
            var moveDirection = Vector2Int.left;

            if (movementValidator.IsValidHorizontalMovement(bubbleSet, moveDirection))
            {
                MoveBubbles(moveDirection);

                return true;
            }

            return false;
        }

        private bool ValidateAndMoveRight()
        {
            var moveDirection = Vector2Int.right;

            if (movementValidator.IsValidHorizontalMovement(bubbleSet, moveDirection))
            {
                MoveBubbles(moveDirection);

                return true;
            }

            return false;
        }

        public bool ValidateAndMoveDown()
        {
            var moveDirection = Vector2Int.down;

            if (movementValidator.isValidDownMovement(bubbleSet))
            {
                MoveBubbles(moveDirection);

                return true;
            }

            return false;
        }

        private void MoveBubbles(Vector2Int moveDirection)
        {
            bubbleSet.Main?.MovementController.MoveDirection(moveDirection);
            bubbleSet.Sub?.MovementController.MoveDirection(moveDirection);
        }

        private void TurnClockwise(int x, int y, Orientation subPos)
        {
            if (bubbleSet.Sub == null)
                return;

            switch (subPos)
            {
                case Orientation.Bottom:
                    if (x - 1 >= 0 && grid.cells[x - 1, y] != null)
                        break;

                    if (x != 0 || ValidateAndMoveRight())
                        orientationController.SetLeftOrientation(bubbleSet.Main, bubbleSet.Sub);

                    break;
                case Orientation.Top:
                    if (x + 1 < gridSize.x && grid.cells[x + 1, y] != null)
                        break;

                    if (x < gridSize.x - 1 || ValidateAndMoveLeft())
                        orientationController.SetRightOrientation(bubbleSet.Main, bubbleSet.Sub);

                    break;
                case Orientation.Left:
                    if (y + 1 < gridSize.y && grid.cells[x, y + 1] != null)
                        break;

                    orientationController.SetTopOrientation(bubbleSet.Main, bubbleSet.Sub);
                    break;
                case Orientation.Right:
                    if (y - 1 >= 0 && grid.cells[x, y - 1] != null)
                        break;

                    orientationController.SetBottomOrientation(bubbleSet.Main, bubbleSet.Sub);
                    break;
                default:
                    break;
            }
        }

        private void TurnCounterClockwise(int x, int y, Orientation subPos)
        {
            switch (subPos)
            {
                case Orientation.Bottom:
                    if (x + 1 < gridSize.x && grid.cells[x + 1, y] != null)
                        break;

                    if (x < gridSize.x - 1 || ValidateAndMoveLeft())
                        orientationController.SetRightOrientation(bubbleSet.Main, bubbleSet.Sub);

                    break;
                case Orientation.Top:
                    if (x - 1 >= 0 && grid.cells[x - 1, y] != null)
                        break;

                    if (x != 0 || ValidateAndMoveRight())
                        orientationController.SetLeftOrientation(bubbleSet.Main, bubbleSet.Sub);

                    break;
                case Orientation.Left:
                    if (y - 1 >= 0 && grid.cells[x, y - 1] != null)
                        break;

                    orientationController.SetBottomOrientation(bubbleSet.Main, bubbleSet.Sub);
                    break;
                case Orientation.Right:
                    if (y + 1 < gridSize.y && grid.cells[x, y + 1] != null)
                        break;

                    orientationController.SetTopOrientation(bubbleSet.Main, bubbleSet.Sub);
                    break;
                default:
                    break;
            }
        }

        public bool StartGame()
        {
            return inputProcessor.StartGame();
        }
    }
}