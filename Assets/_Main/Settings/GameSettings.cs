using System.Collections;
using UnityEngine;

namespace EdwinGameDev.BubbleTeaMatch4
{
    [CreateAssetMenu(fileName = "Settings", menuName = "ScriptableObjects/Settings")]
    public class GameSettings : ScriptableObject
    {
        [SerializeField] private int cellSize = 8;
        [SerializeField] private Vector2Int gridSize = new Vector2Int(8,16);
        [SerializeField] private BubbleSettings bubbleSettings;
        [SerializeField] private Vector2Int spawnPosition = new Vector2Int(3, 16);
        [SerializeField] private Vector2Int nextBubblePosition = new Vector2Int(9, 12);
        [SerializeField] private int bubblesAmountForMatch = 4;
        [SerializeField] private float dropRate = 1;

        public int CellSize => cellSize;
        public Vector2Int GridSize => gridSize;

        public BubbleSettings BubbleSettings => bubbleSettings;

        public Vector2Int MainBubbleSpawnPosition => spawnPosition;
        public Vector2Int SubBubbleSpawnPosition => spawnPosition + Vector2Int.up;

        public Vector2Int NextMainBubblePosition => nextBubblePosition;
        public Vector2Int NextSubBubblePosition => nextBubblePosition + Vector2Int.up;

        public float DropRate => dropRate;
        public int BubblesAmountForMatch => bubblesAmountForMatch;
    }
}