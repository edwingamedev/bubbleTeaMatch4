using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EdwinGameDev.BubbleTeaMatch4
{
    public class Bubble : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer spriteRenderer;
        public BubbleSettings bubbleSettings { get; set; }
        private Vector2Int position;
        private Orientation bubblePosition = Orientation.Top;
        private ConnectionOrientation connection = ConnectionOrientation.none;
        public ConnectionOrientation Connection { get => connection; }
        public Orientation Orientation { get => bubblePosition; set => bubblePosition = value; }

        public int BubbleGroup { get; set; }

        public void Disconnect()
        {
            connection = ConnectionOrientation.none;
        }

        public void Connect(ConnectionOrientation newConnection)
        {
            connection = newConnection;
        }

        public void DisableHighlight()
        {
            spriteRenderer.material.SetFloat("_HighLight", 0);
        }

        public void EnableHighlight()
        {
            spriteRenderer.material.SetFloat("_HighLight", 1);
        }

        public void SetMaterial(Material material)
        {
            spriteRenderer.material = material;
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

        public Vector2Int GetPosition()
        {
            return position;
        }

        public void UpdateGraphics()
        {
            spriteRenderer.sprite = bubbleSettings.SpriteConnections.GetSprite(connection);
        }
    }
}