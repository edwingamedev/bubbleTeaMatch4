using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EdwinGameDev.BubbleTeaMatch4
{
    public class Bubble : MonoBehaviour, IPool
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

        public bool isEnabled()
        {
            return gameObject.activeInHierarchy;
        }

        public void EnableObject()
        {
            Reset();

            gameObject.SetActive(true);
        }
        public void DisableObject()
        {
            ConnectionController.Disconnect();
            gameObject.SetActive(false);
        }

        public GameObject GetObject()
        {
            return gameObject;
        }
    }
}