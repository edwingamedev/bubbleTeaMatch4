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

        public StandardBubbleBuilder(GameSettings settings, Transform poolTransform)
        {
            this.settings = settings;
            bubbleAmount = settings.BubbleSettings.BubblePresets.Count;

            pooling = new Pooling(poolTransform);
            pooling.CreatePool(settings.BubbleSettings.Prefab.GetComponent<IPool>(), 50);
        }

        public Bubble Generate(Vector2Int position)
        {
            int bubbleIndex = Random.Range(0, bubbleAmount);

            //var go = Object.Instantiate(settings.BubbleSettings.Prefab, (Vector2)position, Quaternion.identity);
            var go = pooling.GetFromPool() as Bubble;
            go.MovementController.SetPosition(position);

            var bubble = go.GetComponent<Bubble>();
            bubble.BubbleGroup = bubbleIndex;

            go.name = $"Bubble_{++currenNumOfBubbles}";

            Material material = new Material(settings.BubbleSettings.Shader);
            material.SetColor("_MainColor", settings.BubbleSettings.BubblePresets[bubbleIndex].mainColor);
            material.SetColor("_EyeColor", settings.BubbleSettings.BubblePresets[bubbleIndex].eyeColor);

            bubble.GraphicsController.SetMaterial(material);

            return bubble;
        }

        public void Reset()
        {
            pooling.DisableObjects();
        }
    }
}