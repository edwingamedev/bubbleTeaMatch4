using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EdwinGameDev.BubbleTeaMatch4
{
    public class StandardGridBuilder : IGridBuilder
    {
        private GameSettings settings;
        private Grid grid;
        public Grid Grid => grid;


        public StandardGridBuilder(GameSettings settings)
        {
            this.settings = settings;
        }

        public void Build()
        {
            grid = new Grid(settings.GridSize);
        }
    }
}