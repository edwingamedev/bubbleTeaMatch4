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
        private int currenNumOfEvil;
        private Pooling bubblePooling;
        private Pooling evilPooling;
        private Vector2Int boardOffset;

        public StandardBubbleBuilder(GameSettings settings, Vector2Int boardOffset, Pooling bubblePooling, Pooling evilPooling)
        {
            this.settings = settings;
            bubbleAmount = settings.BubbleSettings.BubblePresets.Count;
            this.boardOffset = boardOffset;
            this.bubblePooling = bubblePooling;
            this.evilPooling = evilPooling;
        }

        public Bubble GenerateEvilBubble(Vector2Int position)
        {
            var go = evilPooling.GetFromPool() as Bubble;
            go.MovementController.SetOffSet(boardOffset);
            go.MovementController.SetPosition(position);

            var bubble = go.GetComponent<Bubble>();
            bubble.bubbleGroup = -1;

            go.name = $"Bubble_Evil_#{++currenNumOfEvil}";

            Material material = new Material(settings.BubbleSettings.Shader);
            material.SetColor("_MainColor", settings.BubbleSettings.EvilBubblePreset.mainColor);
            material.SetColor("_Detail", settings.BubbleSettings.EvilBubblePreset.detail);

            bubble.GraphicsController.SetMaterial(material);

            return bubble;
        }

        public Bubble Generate(Vector2Int position)
        {
            int bubbleIndex = Random.Range(0, bubbleAmount);

            var go = bubblePooling.GetFromPool() as Bubble;
            go.MovementController.SetOffSet(boardOffset);
            go.MovementController.SetPosition(position);

            var bubble = go.GetComponent<Bubble>();
            bubble.bubbleGroup = bubbleIndex;

            go.name = $"Bubble_{bubbleIndex}_#{++currenNumOfBubbles}";

            Material material = new Material(settings.BubbleSettings.Shader);
            material.SetColor("_MainColor", settings.BubbleSettings.BubblePresets[bubbleIndex].mainColor);
            material.SetColor("_Detail", settings.BubbleSettings.BubblePresets[bubbleIndex].detail);

            bubble.GraphicsController.SetMaterial(material);

            return bubble;
        }

        public void Reset()
        {
            bubblePooling.DisableObjects();
        }
    }
}