using UnityEngine;

namespace EdwinGameDev.BubbleTeaMatch4
{
    [System.Serializable]
    public class Bubble : MonoBehaviour, IPool
    {
        public GameSettings gameSettings;
        public IGraphicsController GraphicsController;
        public IMovementController MovementController { get; private set; }
        public IConnectionController ConnectionController { get; private set; }

        public int bubbleGroup;

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
            MovementController = GetComponent<IMovementController>();
            ConnectionController = GetComponent<IConnectionController>();
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
            gameObject.SetActive(true);

            Reset();
        }

        public void DisableObject()
        {
            gameObject.SetActive(false);
        }

        public GameObject GetObject()
        {
            return gameObject;
        }
    }
}