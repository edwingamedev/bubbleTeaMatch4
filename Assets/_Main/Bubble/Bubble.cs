using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EdwinGameDev.BubbleTeaMatch4
{
    public class Bubble : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer spriteRenderer;
        private Vector2Int position;

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

        public Vector2Int GetPosition()
        {
            return position;
        }
    }
}