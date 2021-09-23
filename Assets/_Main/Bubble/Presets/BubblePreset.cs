using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Bubble", menuName = "ScriptableObjects/BubblePreset")]
public class BubblePreset : ScriptableObject
{
    public Color mainColor;
    public Color eyeColor;
}
