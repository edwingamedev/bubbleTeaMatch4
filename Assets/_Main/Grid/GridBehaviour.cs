using System;
using UnityEngine;

namespace EdwinGameDev.BubbleTeaMatch4
{
    public class GridBehaviour
    {
        public Grid Grid { get; private set; }
        private IGridBuilder gridBuilder;

        private GameSettings gameSettings;

        public void ResetGrid()
        {
            Grid.ResetGrid();
        }

        public GridBehaviour(GameSettings gameSettings)
        {
            this.gameSettings = gameSettings;
            gridBuilder = new StandardGridBuilder(gameSettings);
            Grid = gridBuilder.BuildNewGrid();
        }

        public bool ValidateBubbleMovement(BubbleSet bubbleSet)
        {
            return !ReachedBottom(bubbleSet);
        }

        public bool OutOfBounds(BubbleSet bubbleSet)
        {
            var mainPos = bubbleSet.Main.MovementController.GetPosition();
            var subPos = bubbleSet.Sub.MovementController.GetPosition();

            return mainPos.y >= gameSettings.GridSize.y && Grid.IsOccupied(mainPos.x, mainPos.y >= Grid.Size.y ? mainPos.y - 1 : mainPos.y) ||
                   subPos.y >= gameSettings.GridSize.y && Grid.IsOccupied(subPos.x, subPos.y >= Grid.Size.y ? subPos.y-1 : subPos.y);
        }

        public bool ReachedBottom(BubbleSet bubbleSet)
        {
            var reachedBottom = BubbleReachedBottom(bubbleSet.Main) || BubbleReachedBottom(bubbleSet.Sub);

            if (reachedBottom)
            {
                Grid.AssignBubble(bubbleSet.Main, bubbleSet.Main.MovementController.GetPosition().x, bubbleSet.Main.MovementController.GetPosition().y);
                Grid.AssignBubble(bubbleSet.Sub, bubbleSet.Sub.MovementController.GetPosition().x, bubbleSet.Sub.MovementController.GetPosition().y);
            }

            return reachedBottom;
        }

        private bool BubbleReachedBottom(Bubble bubble)
        {
            return Grid.ReachedBottom(bubble);
        }
    }
}