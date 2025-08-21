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
            for (int y = 1; y < sessionVariables.gameSettings.GridSize.y; y++)
            {
                for (int x = 0; x < sessionVariables.gameSettings.GridSize.x; x++)
                {
                    if (sessionVariables.gridBehaviour.Grid.IsOccupied(x, y))
                    {
                        if (!sessionVariables.gridBehaviour.Grid.IsOccupied(x, y - 1))
                        {
                            Bubble bubble = sessionVariables.gridBehaviour.Grid.GetBubble(x, y);
                            bubble.MovementController.MoveDirection(Vector2Int.down);

                            sessionVariables.gridBehaviour.Grid.UnnassignBubble(x, y);
                            sessionVariables.gridBehaviour.Grid.AssignBubble(bubble, x, y - 1);

                            y = 1;
                            x = -1;
                        }
                    }
                }

                Debug.Log(arrangeDelay);
                yield return new WaitForSeconds(arrangeDelay);
            }

            sessionVariables.BubbleRearranged = true;
        }
       
        public void OnExit() { }

        public void Tick() { }
    }
}