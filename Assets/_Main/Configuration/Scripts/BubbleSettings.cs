using System.Collections.Generic;
using UnityEngine;

namespace EdwinGameDev.BubbleTeaMatch4
{
    [CreateAssetMenu(fileName = "BubbleSettings", menuName = "ScriptableObjects/BubbleSettings")]
    public class BubbleSettings : ScriptableObject
    {
        [SerializeField] private GameObject prefab;
        [SerializeField] private GameObject evilPrefab;
        [SerializeField] private Shader shader;
        [SerializeField] private List<ColorPreset> bubblePresets;
        [SerializeField] private ColorPreset evilBubblePreset;
        [SerializeField] private BubbleSpriteConnections spriteConnections;
        public List<ColorPreset> BubblePresets => bubblePresets;

        public Shader Shader => shader;
        public GameObject Prefab => prefab;
        public GameObject EvilPrefab => evilPrefab;
        public ColorPreset EvilBubblePreset => evilBubblePreset;
        public BubbleSpriteConnections SpriteConnections => spriteConnections;
    }
}