using System.Collections.Generic;
using UnityEngine;

namespace EdwinGameDev.BubbleTeaMatch4
{
    [CreateAssetMenu(fileName = "ObjectColorPresets", menuName = "ScriptableObjects/ObjectColorPresets")]
    public class ObjectColorPresets : ScriptableObject
    {
        [SerializeField] private Shader shader;
        public Shader Shader => shader;
        [SerializeField] private List<ColorPreset> colorPresets;
        public List<ColorPreset> ColorPresets => colorPresets;
    }
}