using UnityEngine;

namespace EdwinGameDev.BubbleTeaMatch4
{
    public class BubbleMovementController : MonoBehaviour, IMovementController
    {
        [SerializeField] private Orientation orientation = Orientation.Top;
        private Vector2Int position;
        private Vector2Int offset = Vector2Int.zero;
        public Orientation Orientation { get => orientation; set => orientation = value; }

        public void SetOffSet(Vector2Int offset)
        {
            this.offset = offset;
        }

        public Vector2Int GetPosition()
        {
            return position - offset;
        }

        public void SetPosition(Vector2Int position)
        {
            position = position + offset;
            this.position = position;
            transform.position = new Vector2(position.x, position.y);
        }

        public void MoveDirection(Vector2Int direction)
        {
            SetPosition(GetPosition() + direction);
        }

        public void Reset()
        {
            Orientation = Orientation.Top;
        }
    }
}