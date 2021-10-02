using System;
using UnityEngine;

namespace EdwinGameDev.BubbleTeaMatch4
{
    public class GridBehaviour
    {
        public Grid Grid { get; private set; }
        private IGridBuilder gridBuilder;

        private GameSettings gameSettings;

        public GridBehaviour (GameSettings gameSettings)
        {
            this.gameSettings = gameSettings;
            gridBuilder = new StandardGridBuilder(gameSettings);
            Grid = gridBuilder.BuildNewGrid();
        }

        public bool ValidateBubbleMovement(BubbleSet bubbleSet, Action OnReachedBottom)
        {
            if (!ReachedBottom(bubbleSet))
            {
                return true;
            }
            else
            {
                Grid.AssignBubble(bubbleSet.Main);
                Grid.AssignBubble(bubbleSet.Sub);

                OnReachedBottom?.Invoke();

                return false;
            }
        }

        public bool InBounds(BubbleSet bubbleSet)
        {
            return bubbleSet.Main.MovementController.GetPosition().y >= gameSettings.GridSize.y ||
                    bubbleSet.Sub.MovementController.GetPosition().y >= gameSettings.GridSize.y;
        }

        public bool ReachedBottom(BubbleSet bubbleSet)
        {
            return BubbleReachedBottom(bubbleSet.Main) || BubbleReachedBottom(bubbleSet.Sub);
        }

        private bool BubbleReachedBottom(Bubble bubble)
        {
            return Grid.ReachedBottom(bubble.MovementController.GetPosition());
        }
    }
}