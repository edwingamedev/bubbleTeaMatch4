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

        private float dropRate = 1f;
        private float nextDrop;

        private Bubble mainBubble;
        private Bubble subBubble;

        private Bubble nextMainBubble;
        private Bubble nextSubBubble;

        private GameState gameState = GameState.Initialize;

        bool isArranging = false;

        // Start is called before the first frame update
        void Start()
        {
            Initialize();
        }

        // Update is called once per frame
        void Update()
        {
            switch (gameState)
            {
                case GameState.Initialize:
                    Initialize();
                    break;
                case GameState.Creating:
                    SpawnNewBubbleSet();
                    break;
                case GameState.Playing:
                    BubbleDrop();
                    PlayerMovement();
                    break;
                case GameState.Arrange:
                    if (!isArranging)
                        StartCoroutine(ArrangeBubbles());
                    break;
                case GameState.Linking:
                    gameState = GameState.Creating;
                    break;
                case GameState.Combo:
                    break;
                case GameState.Pause:
                    break;
                default:
                    break;
            }
        }

        private void Initialize()
        {
            gameState = GameState.Initialize;

            bubbleBuilder = new StandardBubbleBuilder(gameSettings);
            gridBuilder = new StandardGridBuilder(gameSettings);
            gridBuilder.Build();

            bubbleMovementController = new BubbleMovementController(gameSettings, gridBuilder.Grid);

            // Start Game
            gameState = GameState.Creating;
        }

        private void SpawnNewBubbleSet()
        {
            // Spawn new Set
            if (nextMainBubble == null)
            {
                // Spawn main
                mainBubble = bubbleBuilder.Generate(gameSettings.MainBubbleSpawnPosition, true);
                // Spawn sub
                subBubble = bubbleBuilder.Generate(gameSettings.SubBubbleSpawnPosition, false);
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
            nextMainBubble = bubbleBuilder.Generate(gameSettings.NextMainBubblePosition, true);
            nextSubBubble = bubbleBuilder.Generate(gameSettings.NextSubBubblePosition, false);


            // Start Game
            gameState = GameState.Playing;
        }

        private bool BubbleReachedBottom(Bubble bubble)
        {
            return gridBuilder.Grid.ReachedBottom(bubble.GetPosition());
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
            gameState = GameState.Pause;
            Debug.Log("GAME OVER");
        }

        private bool CheckGameOver()
        {
            return mainBubble.GetPosition().y >= gameSettings.GridSize.y || subBubble.GetPosition().y >= gameSettings.GridSize.y;
        }

        private void PlayerMovement()
        {
            if (!ReachedBottom())
            {
                if (Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    bubbleMovementController.MoveLeft();
                }

                if (Input.GetKeyDown(KeyCode.RightArrow))
                {
                    bubbleMovementController.MoveRight();
                }

                if (Input.GetKeyDown(KeyCode.DownArrow))
                {
                    bubbleMovementController.MoveDown();
                }

                if (Input.GetKeyDown(KeyCode.Z))
                {
                    bubbleMovementController.TurnClockwise();
                }

                if (Input.GetKeyDown(KeyCode.X))
                {
                    bubbleMovementController.TurnCounterClockwise();
                }
            }
        }

        private IEnumerator ArrangeBubbles()
        {
            isArranging = true;

            for (int y = 1; y < gameSettings.GridSize.y; y++)
            {
                for (int x = 0; x < gameSettings.GridSize.x; x++)
                {
                    if (gridBuilder.Grid.cells[x, y] != null)
                    {
                        if (gridBuilder.Grid.cells[x, y - 1] == null)
                        {
                            Bubble bubble = gridBuilder.Grid.cells[x, y];
                            bubble.MoveDirection(Vector2Int.down);
                            gridBuilder.Grid.cells[x, y - 1] = gridBuilder.Grid.cells[x, y];
                            gridBuilder.Grid.cells[x, y] = null;
                            y = 1;
                            x = -1;
                        }
                    }
                }

                yield return new WaitForEndOfFrame();
            }

            isArranging = false;

            gameState = GameState.Linking;
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

                    gameState = GameState.Arrange;
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