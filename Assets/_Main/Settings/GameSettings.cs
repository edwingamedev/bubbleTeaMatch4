using System.Collections;
using UnityEngine;

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
    public Vector2Int SecondaryBubbleSpawnPosition => spawnPosition + Vector2Int.up;
}