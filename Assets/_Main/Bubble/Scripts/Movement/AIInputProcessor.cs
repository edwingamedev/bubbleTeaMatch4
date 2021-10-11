using UnityEngine;
using System.Threading.Tasks;
using System;

namespace EdwinGameDev.BubbleTeaMatch4
{
    public class AIInputProcessor : IInputProcessor
    {
        public Action OnTurnClockwise { get; set; }
        public Action OnTurnCounterClockwise { get; set; }
        public Action<Vector2Int> OnMove { get; set; }

        private float inputDelay = .5f;
        private float nextInput;
        

        public void CheckInputs()
        {
            if (Time.time > nextInput)
            {
                nextInput = Time.time + inputDelay;

                RandomDecision();
            }
        }

        private void RandomDecision()
        {            
            int random = UnityEngine.Random.Range(0, 5);

            switch (random)
            {
                case 0:
                    OnMove(Vector2Int.down);
                    break;
                case 1:
                    OnMove(Vector2Int.left);
                    break;
                case 2:
                    OnMove(Vector2Int.right);
                    break;
                case 3:
                    OnTurnClockwise();
                    break;
                case 4:
                    OnTurnCounterClockwise();
                    break;
                default:
                    break;
            }
        }
    }
}