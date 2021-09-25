using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EdwinGameDev.BubbleTeaMatch4
{
    public class StandardBubbleBuilder : IBubbleBuilder
    {
        private GameSettings settings;
        private int bubbleAmount;

        public StandardBubbleBuilder(GameSettings settings)
        {
            this.settings = settings;
            bubbleAmount = settings.BubbleSettings.BubblePresets.Count;
        }

        public Bubble Generate(Vector2Int position)
        {
            int bubbleIndex = Random.Range(0, bubbleAmount);

            var go = Object.Instantiate(settings.BubbleSettings.Prefab, (Vector2)position, Quaternion.identity);
            var spriteRenderer = go.GetComponentInChildren<SpriteRenderer>();
            spriteRenderer.material = new Material(settings.BubbleSettings.Shader);
            spriteRenderer.material.SetColor("_MainColor", settings.BubbleSettings.BubblePresets[bubbleIndex].mainColor);
            spriteRenderer.material.SetColor("_EyeColor", settings.BubbleSettings.BubblePresets[bubbleIndex].eyeColor);

            var bubble = go.GetComponent<Bubble>();

            return bubble;
        }
    }
}