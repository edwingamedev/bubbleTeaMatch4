using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EdwinGameDev.BubbleTeaMatch4
{
    public class StandardBubbleBuilder : IBubbleBuilder
    {
        private GameSettings settings;
        private int bubbleAmount;
        private int currenNumOfBubbles;

        public StandardBubbleBuilder(GameSettings settings)
        {
            this.settings = settings;
            bubbleAmount = settings.BubbleSettings.BubblePresets.Count;
        }

        public Bubble Generate(Vector2Int position)
        {
            int bubbleIndex = Random.Range(0, bubbleAmount);

            var go = Object.Instantiate(settings.BubbleSettings.Prefab, (Vector2)position, Quaternion.identity);
            var bubble = go.GetComponent<Bubble>();
            bubble.BubbleGroup = bubbleIndex;

            go.name = $"Bubble_{++currenNumOfBubbles}";

            Material material = new Material(settings.BubbleSettings.Shader);
            material.SetColor("_MainColor", settings.BubbleSettings.BubblePresets[bubbleIndex].mainColor);
            material.SetColor("_EyeColor", settings.BubbleSettings.BubblePresets[bubbleIndex].eyeColor);

            bubble.GraphicsController.SetMaterial(material);      

            return bubble;
        }
    }
}