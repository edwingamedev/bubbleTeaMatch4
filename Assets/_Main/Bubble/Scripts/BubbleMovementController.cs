using UnityEngine;

namespace EdwinGameDev.BubbleTeaMatch4
{
    public class BubbleMovementController : MonoBehaviour, IMovementController
    {
        [SerializeField] private Orientation orientation = Orientation.Top;
        private Vector2Int position;

        public Orientation Orientation { get => orientation; set => orientation = value; }

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

        public void Reset()
        {
            Orientation = Orientation.Top;
        }
    }
}