using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EdwinGameDev.BubbleTeaMatch4
{
    public class Bubble : MonoBehaviour
    {
        public GameSettings gameSettings;
        public IGraphicsController GraphicsController;
        public IMovementController MovementController { get; private set; }
        public IConnectionController ConnectionController { get; private set; }

        public int BubbleGroup { get; set; }

        private void Awake()
        {
            Initialize();
        }
        public void Reset()
        {
            MovementController.Reset();
            ConnectionController.Reset();

            UpdateGraphics();
        }

        private void Initialize()
        {
            GraphicsController = GetComponent<IGraphicsController>();
            MovementController = new BubbleMovementController(transform);
            ConnectionController = new BubbleConnectionController(gameSettings);
        }

        public void UpdateGraphics()
        {
            GraphicsController.UpdateGraphics(ConnectionController.Connection);
        }
    }
}