using UnityEngine;

namespace EdwinGameDev.BubbleTeaMatch4
{
    public class BubbleMovementController : IMovementController
    {               
        private Orientation bubblePosition = Orientation.Top;
        public Orientation Orientation { get => bubblePosition; set => bubblePosition = value; }
        private Vector2Int position;
        private Transform transform;

        public BubbleMovementController(Transform bubbleTransform)
        {
            this.transform = bubbleTransform;
        }

        public Vector2Int GetPosition()
        {
            return position;
        }

        public void SetPosition(Vector2Int position)
        {
            this.position = position;
            transform.position = new Vector2(position.x, position.y);
        }

        public void MoveDirection(Vector2Int direction)
        {
            SetPosition(new Vector2Int(Mathf.CeilToInt(transform.position.x) + direction.x,
                                        Mathf.CeilToInt(transform.position.y) + direction.y));
        }
    }
}