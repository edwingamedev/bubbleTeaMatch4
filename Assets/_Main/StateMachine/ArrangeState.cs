using System.Threading.Tasks;
using UnityEngine;

namespace EdwinGameDev.BubbleTeaMatch4
{
    public class ArrangeState : IState
    {
        private GameBoard gameVariables;
        private readonly int arrangeDelay = 5;

        public ArrangeState(GameBoard gameVariables)
        {
            this.gameVariables = gameVariables;
        }

        public void OnEnter()
        {
            gameVariables.BubbleRearranged = false;

            ArrangeBubbles();
        }

        private async void ArrangeBubbles()
        {
            for (int y = 1; y < gameVariables.gameSettings.GridSize.y; y++)
            {
                for (int x = 0; x < gameVariables.gameSettings.GridSize.x; x++)
                {
                    if (gameVariables.gridBehaviour.Grid.IsOccupied(x, y))
                    {
                        if (!gameVariables.gridBehaviour.Grid.IsOccupied(x, y - 1))
                        {
                            Bubble bubble = gameVariables.gridBehaviour.Grid.GetBubble(x, y);
                            bubble.MovementController.MoveDirection(Vector2Int.down);

                            gameVariables.gridBehaviour.Grid.UnnassignBubble(x, y);
                            gameVariables.gridBehaviour.Grid.AssignBubble(bubble, x, y - 1);

                            y = 1;
                            x = -1;
                        }
                    }
                }

                await Task.Delay(arrangeDelay);
            }

            UnityEngine.Debug.Log("ArrangeBubbles");
            gameVariables.BubbleRearranged = true;
        }

        public void OnExit() { }

        public void Tick() { }
    }
}