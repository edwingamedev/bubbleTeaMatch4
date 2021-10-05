using UnityEngine;
using System.Threading.Tasks;

namespace EdwinGameDev.BubbleTeaMatch4
{
    public class AIInputProcessor : IInputProcessor
    {
        private bool moveDown;
        private bool moveLeft;
        private bool moveRight;
        private bool turnCounterClockwise;
        private bool turnClockwise;

        private int delayBetweenDecisions = 500;

        public AIInputProcessor()
        {
            RandomDecision();
        }

        private async void RandomDecision()
        {
            await Task.Delay(delayBetweenDecisions);

            int random = Random.Range(0, 5);

            switch (random)
            {
                case 0:
                    moveDown = true;
                    break;
                case 1:
                    moveLeft = true;
                    break;
                case 2:
                    moveRight = true;
                    break;
                case 3:
                    turnClockwise = true;
                    break;
                case 4:
                    turnCounterClockwise = true;
                    break;
                default:
                    break;
            }            

            RandomDecision();
        }

        public bool Down()
        {
            if (moveDown)
            {
                moveDown = false;
                return true;
            }                

            return false;
        }

        public bool Left()
        {
            if (moveLeft)
            {
                moveLeft = false;
                return true;
            }

            return false;
        }

        public bool Right()
        {
            if (moveRight)
            {
                moveRight = false;
                return true;
            }
                
            return false;
        }

        public bool TurnClockwise()
        {
            if (turnClockwise)
            {
                turnClockwise = false;
                return true;
            }

            return false;
        }

        public bool TurnCounterClockwise()
        {
            if (turnCounterClockwise)                
            {
                turnCounterClockwise = false;
                return true;
            }

            return false;
        }
    }
}