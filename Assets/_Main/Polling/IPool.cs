using UnityEngine;

namespace EdwinGameDev.BubbleTeaMatch4
{
    public interface IPool
    {
        bool isEnabled();
        void EnableObject();
        void DisableObject();
        GameObject GetObject();
    }
}