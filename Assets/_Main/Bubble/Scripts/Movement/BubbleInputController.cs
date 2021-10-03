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
        private Vector2Int moveVector = Vector2Int.zero;

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
                    moveVector += Vector2Int.left;
                }

                if (inputProcessor.Right())
                {
                    moveVector += Vector2Int.right;
                }

                if (inputProcessor.Down())
                {
                    moveVector += Vector2Int.down;
                }

                if (moveVector != Vector2Int.zero)
                {
                    // Validate Movement
                    ValidateVerticalMovement(moveVector);
                    ValidateHorizontalMovement(moveVector);
                }

                // Reset Movement
                moveVector = Vector2Int.zero;
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

        private bool ValidateVerticalMovement(Vector2Int moveDirection)
        {
            return ValidateAndMove(new Vector2Int(0, moveDirection.y));
        }

        private bool ValidateHorizontalMovement(Vector2Int moveDirection)
        {
            return ValidateAndMove(new Vector2Int(moveDirection.x, 0));
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

        private void TurnClockwise(int x, int y, Orientation subPos)
        {
            if (bubbleSet.Sub == null)
                return;

            switch (subPos)
            {
                case Orientation.Bottom:
                    if (x - 1 >= 0 && grid.cells[x - 1, y] != null)
                        break;

                    if (x != 0 || ValidateAndMove(Vector2Int.right))
                        orientationController.SetLeftOrientation(bubbleSet.Main, bubbleSet.Sub);

                    break;
                case Orientation.Top:
                    if (x + 1 < gridSize.x && grid.cells[x + 1, y] != null)
                        break;

                    if (x < gridSize.x - 1 || ValidateAndMove(Vector2Int.left))
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

                    if (x < gridSize.x - 1 || ValidateAndMove(Vector2Int.left))
                        orientationController.SetRightOrientation(bubbleSet.Main, bubbleSet.Sub);

                    break;
                case Orientation.Top:
                    if (x - 1 >= 0 && grid.cells[x - 1, y] != null)
                        break;

                    if (x != 0 || ValidateAndMove(Vector2Int.right))
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