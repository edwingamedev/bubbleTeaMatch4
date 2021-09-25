using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EdwinGameDev.BubbleTeaMatch4
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private GameSettings gameSettings;
        private IGridBuilder gridBuilder;
        private IBubbleBuilder bubbleBuilder;
        private BubbleMovementController bubbleMovementController;
        private bool isPlaying = false;

        private float dropRate = 1f;
        private float nextDrop;

        private Bubble mainBubble;
        private Bubble subBubble;

        private Bubble nextMainBubble;
        private Bubble nextSubBubble;

        // Start is called before the first frame update
        void Start()
        {
            Initialize();
        }

        private void Initialize()
        {
            bubbleBuilder = new StandardBubbleBuilder(gameSettings);
            gridBuilder = new StandardGridBuilder(gameSettings);
            gridBuilder.Build();

            bubbleMovementController = new BubbleMovementController(gameSettings, gridBuilder.Grid);           

            // Spawn bubble set
            SpawnNewBubbleSet();

            // Start Game
            isPlaying = true;
        }

        private void SpawnNewBubbleSet()
        {
            // Spawn new Set
            if (nextMainBubble == null)
            {
                // Spawn main
                mainBubble = bubbleBuilder.Generate(gameSettings.MainBubbleSpawnPosition);
                // Spawn sub
                subBubble = bubbleBuilder.Generate(gameSettings.SubBubbleSpawnPosition);
            }
            else
            {
                // Spawn main
                mainBubble = nextMainBubble;
                // Spawn sub
                subBubble = nextSubBubble;
            }

            // Set Initial Position
            mainBubble.SetPosition(gameSettings.MainBubbleSpawnPosition + Vector2Int.down);
            subBubble.SetPosition(gameSettings.SubBubbleSpawnPosition + Vector2Int.down);

            bubbleMovementController.SetBubbles(mainBubble, subBubble);

            // Next
            nextMainBubble = bubbleBuilder.Generate(gameSettings.NextMainBubblePosition);
            nextSubBubble = bubbleBuilder.Generate(gameSettings.NextSubBubblePosition);
        }

        private bool BubbleReachedBottom(Bubble bubble)
        {
            return gridBuilder.Grid.ReachedBottom(bubble.GetPosition());
        }

        // Update is called once per frame
        void Update()
        {
            if (isPlaying)
            {
                BubbleDrop();

                PlayerMovement();
            }
        }

        private void BubbleDrop()
        {
            if (Time.time > nextDrop)
            {
                nextDrop = Time.time + dropRate;

                if (!ReachedBottom())
                {
                    bubbleMovementController.MoveDown();
                }
            }
        }

        private void SetGameOver()
        {
            isPlaying = false;
            Debug.Log("GAME OVER");
        }

        private bool CheckGameOver()
        {
            return mainBubble.GetPosition().y >= gameSettings.GridSize.y || subBubble.GetPosition().y >= gameSettings.GridSize.y;
        }

        private void PlayerMovement()
        {
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                if (!ReachedBottom())
                {
                    bubbleMovementController.MoveLeft();
                }
            }

            if (Input.GetKey(KeyCode.RightArrow))
            {
                if (!ReachedBottom())
                {
                    bubbleMovementController.MoveRight();
                }
            }

            if (Input.GetKey(KeyCode.DownArrow))
            {
                if (!ReachedBottom())
                {
                    bubbleMovementController.MoveDown();
                }
            }
        }

        private bool ReachedBottom()
        {
            if (BubbleReachedBottom(mainBubble) || BubbleReachedBottom(subBubble))
            {
                if (CheckGameOver())
                {
                    SetGameOver();
                }
                else
                {
                    // Drop Pieces
                    gridBuilder.Grid.AssignBubble(mainBubble);
                    gridBuilder.Grid.AssignBubble(subBubble);

                    SpawnNewBubbleSet();
                }

                return true;
            }
            else
            {
                return false;
            }
        }
    }
}