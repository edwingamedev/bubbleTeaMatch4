using System.Collections;
using UnityEngine;

namespace EdwinGameDev.BubbleTeaMatch4
{
    [CreateAssetMenu(fileName = "Settings", menuName = "ScriptableObjects/Settings")]
    public class GameSettings : ScriptableObject
    {
        [SerializeField] private int cellSize;
        [SerializeField] private Vector2Int gridSize;
        [SerializeField] private BubbleSettings bubbleSettings;
        [SerializeField] private Vector2Int spawnPosition;
        [SerializeField] private Vector2Int nextBubblePosition;

        public int CellSize => cellSize;
        public Vector2Int GridSize => gridSize;

        public BubbleSettings BubbleSettings => bubbleSettings;

        public Vector2Int MainBubbleSpawnPosition => spawnPosition;
        public Vector2Int SubBubbleSpawnPosition => spawnPosition + Vector2Int.up;

        public Vector2Int NextMainBubblePosition => nextBubblePosition;
        public Vector2Int NextSubBubblePosition => nextBubblePosition + Vector2Int.up;
    }
}