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
            bool isEnemyAttack = false;
            for (int y = 1; y < sessionVariables.gameSettings.GridSize.y; y++)
            {
                for (int x = 0; x < sessionVariables.gameSettings.GridSize.x; x++)
                {
                    if (!sessionVariables.gridBehaviour.Grid.IsOccupied(x, y))
                    {
                        continue;
                    }

                    if (sessionVariables.gridBehaviour.Grid.IsOccupied(x, y - 1))
                    {
                        continue;
                    }

                    Bubble bubble = sessionVariables.gridBehaviour.Grid.GetBubble(x, y);

                    if (bubble.bubbleGroup == -1)
                    {
                        isEnemyAttack = true;
                    }

                    bubble.ConnectionController.Reset();
                    bubble.UpdateGraphics();
                    bubble.MovementController.MoveDirection(Vector2Int.down);

                    sessionVariables.gridBehaviour.Grid.UnnassignBubble(x, y);
                    sessionVariables.gridBehaviour.Grid.AssignBubble(bubble, x, y - 1);

                    y = 1;
                    x = -1;
                }

                yield return new WaitForSeconds(isEnemyAttack ? 0 : arrangeDelay);
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