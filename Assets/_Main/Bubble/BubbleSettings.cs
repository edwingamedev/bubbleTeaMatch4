using System.Collections.Generic;
using UnityEngine;

namespace EdwinGameDev.BubbleTeaMatch4
{
    [CreateAssetMenu(fileName = "BubbleSettings", menuName = "ScriptableObjects/BubbleSettings")]
    public class BubbleSettings : ScriptableObject
    {
        [SerializeField] private GameObject prefab;
        [SerializeField] private Shader shader;
        [SerializeField] private List<BubblePreset> bubblePresets;
        [SerializeField] private BubblePreset evilBubble;

        public List<BubblePreset> BubblePresets => bubblePresets;

        public Shader Shader => shader;
        public GameObject Prefab => prefab;
        public BubblePreset EvilBubble => evilBubble;
    }
}