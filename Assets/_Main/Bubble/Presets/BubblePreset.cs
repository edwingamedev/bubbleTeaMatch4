using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EdwinGameDev.BubbleTeaMatch4
{
    [CreateAssetMenu(fileName = "Bubble", menuName = "ScriptableObjects/BubblePreset")]
    public class BubblePreset : ScriptableObject
    {
        public Color mainColor;
        public Color eyeColor;
    }
}