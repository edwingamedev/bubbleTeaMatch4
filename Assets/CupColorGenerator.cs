using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EdwinGameDev.BubbleTeaMatch4
{
    public class CupColorGenerator : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer teaRenderer;
        [SerializeField] private SpriteRenderer strawRenderer;
        [SerializeField] private GameSettings settings;


        public void Generate()
        {            
            SetColor(teaRenderer, settings.TeaPreset,"_FirstColor", "_SecondColor");
            SetColor(strawRenderer, settings.StrawPreset, "_MainColor", "_Detail");
        }

        private void SetColor(SpriteRenderer spriteRenderer, ObjectColorPresets objectColorPresets, string mainColor, string secondColor)
        {
            var colorIndex = Random.Range(0, objectColorPresets.ColorPresets.Count);

            Material material = new Material(objectColorPresets.Shader);
            material.SetColor(mainColor, objectColorPresets.ColorPresets[colorIndex].mainColor);
            material.SetColor(secondColor, objectColorPresets.ColorPresets[colorIndex].detail);

            spriteRenderer.material = material;
        }
    }
}