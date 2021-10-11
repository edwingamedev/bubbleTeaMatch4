using UnityEngine;

namespace EdwinGameDev.BubbleTeaMatch4
{
    public class BubbleOrientationController : IOrientationController<Bubble>
    {
        public void SetLeftOrientation(Bubble main, Bubble sub)
        {
            sub.MovementController.SetPosition(new Vector2Int(main.MovementController.GetPosition().x - 1, main.MovementController.GetPosition().y));

            // Set New Orientation
            sub.MovementController.Orientation = Orientation.Left;
        }

        public void SetRightOrientation(Bubble main, Bubble sub)
        {
            sub.MovementController.SetPosition(new Vector2Int(main.MovementController.GetPosition().x + 1, main.MovementController.GetPosition().y));

            // Set New Orientation
            sub.MovementController.Orientation = Orientation.Right;
        }

        public void SetTopOrientation(Bubble main, Bubble sub)
        {
            sub.MovementController.SetPosition(new Vector2Int(main.MovementController.GetPosition().x, main.MovementController.GetPosition().y + 1));

            // Set New Orientation
            sub.MovementController.Orientation = Orientation.Top;
        }

        public void SetBottomOrientation(Bubble main, Bubble sub)
        {
            sub.MovementController.SetPosition(new Vector2Int(main.MovementController.GetPosition().x, main.MovementController.GetPosition().y - 1));

            // Set New Orientation
            sub.MovementController.Orientation = Orientation.Bottom;
        }
    }
}