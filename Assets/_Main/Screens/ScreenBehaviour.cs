using UnityEngine;

namespace EdwinGameDev.BubbleTeaMatch4
{
    [System.Serializable]
    public abstract class ScreenBehaviour : MonoBehaviour
    {
        public abstract void OnActivate();

        public abstract void OnDeactivate();
    }
}