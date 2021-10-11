using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EdwinGameDev.BubbleTeaMatch4
{
    [CreateAssetMenu(fileName = "Color", menuName = "ScriptableObjects/ColorPreset")]
    public class ColorPreset : ScriptableObject
    {
        public Color mainColor = Color.gray;        
        public Color detail = Color.white;
    }
}