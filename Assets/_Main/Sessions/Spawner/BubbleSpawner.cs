﻿using System;
using UnityEngine;

namespace EdwinGameDev.BubbleTeaMatch4
{   
    public class BubbleSpawner
    {
        public BubbleSet CurrentSet { get; private set; }

        private BubbleSet nextSet;

        private IBubbleBuilder bubbleBuilder;
        private GameSettings gameSettings;

        public BubbleSpawner(GameSettings gameSettings, Vector2Int boardOffset, Pooling bubblePooling, Pooling evilPooling)
        {
            bubbleBuilder = new StandardBubbleBuilder(gameSettings, boardOffset, bubblePooling, evilPooling);
            this.gameSettings = gameSettings;
            CurrentSet = new BubbleSet();
        }

        public Bubble SpawnEvilBubble(Vector2Int position) 
        {
           return bubbleBuilder.GenerateEvilBubble(position);
        }

        public void SpawnNewBubbleSet()
        {
            // Spawn new Set
            if (nextSet != null)
            {
                // Spawn main
                CurrentSet.Main = nextSet.Main;
                CurrentSet.Sub = nextSet.Sub;
            }
            else
            {
                // Initialize next bubble set
                nextSet = new BubbleSet();

                CurrentSet.Main = bubbleBuilder.Generate(gameSettings.MainBubbleSpawnPosition);
                CurrentSet.Sub = bubbleBuilder.Generate(gameSettings.SubBubbleSpawnPosition);
            }

            // Enable main bubble highlight
            CurrentSet.Main.GraphicsController.EnableHighlight();

            // Set Initial Position
            CurrentSet.Main.MovementController.SetPosition(gameSettings.MainBubbleSpawnPosition + Vector2Int.down);
            CurrentSet.Sub.MovementController.SetPosition(gameSettings.SubBubbleSpawnPosition + Vector2Int.down);

            // Set Next bubbles
            nextSet.Main = bubbleBuilder.Generate(gameSettings.NextMainBubblePosition);
            nextSet.Sub = bubbleBuilder.Generate(gameSettings.NextSubBubblePosition);                        
        }

        public void Reset()
        {
            CurrentSet = new BubbleSet();
            nextSet = null;
            bubbleBuilder.Reset();
        }
    }
}