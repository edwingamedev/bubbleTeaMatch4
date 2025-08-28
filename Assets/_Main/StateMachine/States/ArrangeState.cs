using System.Collections;
using UnityEngine;

namespace EdwinGameDev.BubbleTeaMatch4
{
    public class ArrangeState : IState
    {
        private SessionVariables sessionVariables;
        private readonly float arrangeDelay = 0.005f;

        public ArrangeState(SessionVariables sessionVariables)
        {
            this.sessionVariables = sessionVariables;
        }

        public void OnEnter()
        {
            sessionVariables.BubbleRearranged = false;

            CoroutineRunner.Instance.Run(ArrangeBubbles());
        }

        private IEnumerator ArrangeBubbles()
        {
            var gridSize = sessionVariables.gameSettings.GridSize;
            var grid = sessionVariables.gridBehaviour.Grid;

            for (int x = 0; x < gridSize.x; x++)
            {
                int targetY = 0;

                for (int y = 0; y < gridSize.y; y++)
                {
                    if (!grid.IsOccupied(x, y))
                    {
                        continue;
                    }

                    // Only move if necessary
                    if (y != targetY)
                    {
                        Bubble bubble = grid.GetBubble(x, y);
                        grid.UnassignBubble(x, y);
                        grid.AssignBubble(bubble, x, targetY);
                        bubble.ConnectionController.Reset();
                        bubble.UpdateGraphics();

                        int desiredY = y - targetY;
                        for (int i = 0; i < desiredY; i++)
                        {
                            bubble.MovementController.MoveDirection(Vector2Int.down);
                            yield return new WaitForSeconds(arrangeDelay);
                        }
                    }

                    targetY++; // next bubble stacks above
                }
            }

            sessionVariables.BubbleRearranged = true;
        }

        public void OnExit()
        {
        }

        public void Tick()
        {
        }
    }
}