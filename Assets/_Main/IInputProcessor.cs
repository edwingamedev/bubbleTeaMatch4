using System;
using UnityEngine;

namespace EdwinGameDev.BubbleTeaMatch4
{
    public interface IInputProcessor
    {
        Action OnTurnClockwise { get; set; }
        Action OnTurnCounterClockwise { get; set; }
        Action<Vector2Int> OnMove { get; set; }
        void CheckInputs();
    }
}