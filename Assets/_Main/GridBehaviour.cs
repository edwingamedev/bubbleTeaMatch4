using System;
using UnityEngine;

namespace EdwinGameDev.BubbleTeaMatch4
{
    public class GridBehaviour
    {
        public Grid Grid { get; private set; }
        private IGridBuilder gridBuilder;

        private GameSettings gameSettings;

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
            return bubbleSet?.Main.MovementController.GetPosition().y >= gameSettings?.GridSize.y ||
                    bubbleSet?.Sub.MovementController.GetPosition().y >= gameSettings?.GridSize.y;
        }

        public bool ReachedBottom(BubbleSet bubbleSet)
        {
            var reachedBottom = BubbleReachedBottom(bubbleSet.Main) || BubbleReachedBottom(bubbleSet.Sub);

            if (reachedBottom)
            {
                Grid.AssignBubble(bubbleSet.Main, bubbleSet.Main.MovementController.GetPosition().x, bubbleSet.Main.MovementController.GetPosition().y);
                Grid.AssignBubble(bubbleSet.Sub, bubbleSet.Sub.MovementController.GetPosition().x, bubbleSet.Sub.MovementController.GetPosition().y);

                Debug.Log("ReachedBottom");
            }

            return reachedBottom;
        }

        private bool BubbleReachedBottom(Bubble bubble)
        {
            return Grid.ReachedBottom(bubble);
        }
    }
}