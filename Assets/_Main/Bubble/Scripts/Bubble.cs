using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EdwinGameDev.BubbleTeaMatch4
{
    public class Bubble : MonoBehaviour
    {
        public IGraphicsController GraphicsController;
        public IMovementController MovementController { get; private set; }
        public IConnectionController ConnectionController { get; private set; }

        public int BubbleGroup { get; set; }

        private void Awake()
        {
            MovementController = new BubbleMovementController(transform);
            ConnectionController = new BubbleConnectionController();
            GraphicsController = GetComponent<IGraphicsController>();
        }

        public void UpdateGraphics()
        {
            GraphicsController.UpdateGraphics(ConnectionController.Connection);
        }
    }
}