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
        private Pooling pooling;
        private Vector2Int boardOffset;

        public StandardBubbleBuilder(GameSettings settings, Vector2Int boardOffset, Pooling pooling)
        {
            this.settings = settings;
            bubbleAmount = settings.BubbleSettings.BubblePresets.Count;
            this.boardOffset = boardOffset;
            this.pooling = pooling;
        }

        public Bubble Generate(Vector2Int position)
        {
            int bubbleIndex = Random.Range(0, bubbleAmount);

            //var go = Object.Instantiate(settings.BubbleSettings.Prefab, (Vector2)position, Quaternion.identity);
            var go = pooling.GetFromPool() as Bubble;
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
            pooling.DisableObjects();
        }
    }
}