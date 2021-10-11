using UnityEngine;

namespace EdwinGameDev.BubbleTeaMatch4
{
    public interface IGraphicsController
    {
        void DisableHighlight();
        void EnableHighlight();
        void PopAnimation();
        void ResetAnimation();
        void UpdateGraphics(ConnectionOrientation connection);
        void SetMaterial(Material material);
    }
}