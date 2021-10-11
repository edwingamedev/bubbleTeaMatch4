using System.Threading.Tasks;
using UnityEngine;

namespace EdwinGameDev.BubbleTeaMatch4
{
    public class ArrangeState : IState
    {
        private SessionVariables sessionVariables;
        private readonly int arrangeDelay = 5;

        public ArrangeState(SessionVariables sessionVariables)
        {
            this.sessionVariables = sessionVariables;
        }

        public void OnEnter()
        {
            sessionVariables.BubbleRearranged = false;

            ArrangeBubbles();
        }

        private async void ArrangeBubbles()
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

                await Task.Delay(arrangeDelay);
            }

            sessionVariables.BubbleRearranged = true;
        }

        public void OnExit() { }

        public void Tick() { }
    }
}