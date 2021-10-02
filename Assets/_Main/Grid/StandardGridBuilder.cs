using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EdwinGameDev.BubbleTeaMatch4
{
    public class StandardGridBuilder : IGridBuilder
    {
        private GameSettings settings;

        public StandardGridBuilder(GameSettings settings)
        {
            this.settings = settings;
        }

        public Grid BuildNewGrid()
        {
            return new Grid(settings.GridSize);
        }
    }
}