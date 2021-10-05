using UnityEngine;

namespace EdwinGameDev.BubbleTeaMatch4
{
    public class GameBoard
    {
        public GameSettings gameSettings;
        public GridBehaviour gridBehaviour;
        public BubbleSpawner bubbleSpawner;
        public IInputController inputController;
        public Vector2Int boardOffset;

        public bool GameStarted { get;  set; }
        public bool HasMatches { get; set; }
        public bool BubbleRearranged { get; set; }
        public bool ComboStarted{ get; set; }
    }
}