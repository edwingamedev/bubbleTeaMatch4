using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace EdwinGameDev.BubbleTeaMatch4
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private GameSettings gameSettings;
        private IGridBuilder gridBuilder;
        private IBubbleBuilder bubbleBuilder;
        private BubbleMovementController bubbleMovementController;
        [SerializeField] private ScoreController scoreController;
        private float dropRate = 1f;
        private float nextDrop;

        private Bubble mainBubble;
        private Bubble subBubble;

        private Bubble nextMainBubble;
        private Bubble nextSubBubble;

        private GameState gameState = GameState.Initialize;

        bool isArranging = false;
        bool isPoping = false;

        private float inputDelay = .1f;
        private float nextInput;

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
                    gameState = GameState.Combo;
                    LinkBubbles();
                    break;
                case GameState.Combo:
                    if (isPoping)
                        return;

                    isPoping = true;

                    if (PrepareBubblePop())
                    {
                        StartCoroutine(PopAndFillGap());
                    }
                    else
                    {
                        isPoping = false;
                        gameState = GameState.Creating;
                    }
                    break;
                case GameState.Pause:
                    break;
                default:
                    break;
            }
        }

        private IEnumerator PopAndFillGap()
        {
            yield return new WaitForSeconds(dropRate / 2);

            PopBubbles();
            gameState = GameState.Arrange;

            isPoping = false;
        }

        private void UpdateImage()
        {
            bool emptyRow;
            for (int y = 0; y < gameSettings.GridSize.y; y++)
            {
                emptyRow = true;

                for (int x = 0; x < gameSettings.GridSize.x; x++)
                {
                    if (gridBuilder.Grid.cells[x, y] != null)
                    {
                        emptyRow = false;
                        gridBuilder.Grid.cells[x, y].UpdateGraphics();
                    }
                }
                if (emptyRow)
                    break;
            }
        }

        private void UpdateBubbleConnectionList(Bubble bubbleA, Bubble bubbleB)
        {
            List<Bubble> bubbleAList = bubbleA.GetConnectionList();
            if (!bubbleAList.Contains(bubbleB))
            {
                bubbleAList.Add(bubbleB);
            }

            List<Bubble> bubbleBList = bubbleB.GetConnectionList();
            if (!bubbleBList.Contains(bubbleA))
            {
                bubbleBList.Add(bubbleA);
            }

            List<Bubble> bubbleCList = bubbleAList.Union(bubbleBList).ToList();

            for (int i = 0; i < bubbleAList.Count; i++)
            {
                bubbleAList[i].SetConnectionList(bubbleCList);
            }
            for (int i = 0; i < bubbleBList.Count; i++)
            {
                bubbleBList[i].SetConnectionList(bubbleCList);
            }
        }

        private bool PrepareBubblePop()
        {
            bool hasConnection = false;

            for (int y = 0; y < gameSettings.GridSize.y; y++)
            {
                bool emptyRow = true;
                for (int x = 0; x < gameSettings.GridSize.x; x++)
                {
                    if (gridBuilder.Grid.cells[x, y] != null)
                    {
                        emptyRow = false;

                        if (gridBuilder.Grid.cells[x, y].GetConnectionList().Count >= 4)
                        {
                            hasConnection = true;
                            gridBuilder.Grid.cells[x, y].Matched = true;
                            gridBuilder.Grid.cells[x, y].PopAnimation();
                        }
                    }
                }
                if (emptyRow)
                    break;
            }
            //UpdateImage();
            return hasConnection;
        }

        private void PopBubbles()
        {
            for (int y = 0; y < gameSettings.GridSize.y; y++)
            {
                bool emptyRow = true;
                for (int x = 0; x < gameSettings.GridSize.x; x++)
                {
                    if (gridBuilder.Grid.cells[x, y] != null)
                    {
                        if (gridBuilder.Grid.cells[x, y].Matched)
                        {
                            Destroy(gridBuilder.Grid.cells[x, y].gameObject);
                            gridBuilder.Grid.cells[x, y] = null;

                            scoreController.AddPoints(10);
                        }

                        emptyRow = false;
                    }
                }
                if (emptyRow)
                    break;
            }
        }

        private void LinkBubbles()
        {
            bool emptyRow;

            for (int y = 0; y < gameSettings.GridSize.y; y++)
            {
                emptyRow = true;

                for (int x = 0; x < gameSettings.GridSize.x; x++)
                {
                    if (gridBuilder.Grid.cells[x, y] != null)
                    {
                        emptyRow = false;

                        // Horizontal
                        if (x + 1 < gridBuilder.Grid.cells.GetLength(0) && gridBuilder.Grid.cells[x + 1, y] != null && gridBuilder.Grid.cells[x, y].BubbleGroup == gridBuilder.Grid.cells[x + 1, y].BubbleGroup)
                        {
                            if (gridBuilder.Grid.cells[x, y].Connection == ConnectionOrientation.left)
                                gridBuilder.Grid.cells[x, y].Connect(ConnectionOrientation.left_right);
                            else
                                gridBuilder.Grid.cells[x, y].Connect(ConnectionOrientation.right);


                            gridBuilder.Grid.cells[x + 1, y].Connect(ConnectionOrientation.left);

                            UpdateBubbleConnectionList(gridBuilder.Grid.cells[x, y], gridBuilder.Grid.cells[x + 1, y]);
                        }
                    }
                }

                if (emptyRow)
                    break;
            }

            for (int y = 0; y < gameSettings.GridSize.y; y++)
            {
                emptyRow = true;

                for (int x = 0; x < gameSettings.GridSize.x; x++)
                {
                    if (gridBuilder.Grid.cells[x, y] != null)
                    {
                        emptyRow = false;
                        // Vertical
                        if (y + 1 < gridBuilder.Grid.cells.GetLength(1) && gridBuilder.Grid.cells[x, y + 1] != null && gridBuilder.Grid.cells[x, y].BubbleGroup == gridBuilder.Grid.cells[x, y + 1].BubbleGroup)
                        {
                            switch (gridBuilder.Grid.cells[x, y + 1].Connection)
                            {
                                case ConnectionOrientation.none:
                                    gridBuilder.Grid.cells[x, y + 1].Connect(ConnectionOrientation.bottom);
                                    break;
                                case ConnectionOrientation.top:
                                    gridBuilder.Grid.cells[x, y + 1].Connect(ConnectionOrientation.top_bottom);
                                    break;
                                case ConnectionOrientation.left:
                                    gridBuilder.Grid.cells[x, y + 1].Connect(ConnectionOrientation.bottom_left);
                                    break;
                                case ConnectionOrientation.right:
                                    gridBuilder.Grid.cells[x, y + 1].Connect(ConnectionOrientation.bottom_right);
                                    break;
                                case ConnectionOrientation.left_right:
                                    gridBuilder.Grid.cells[x, y + 1].Connect(ConnectionOrientation.bottom_left_right);
                                    break;
                                case ConnectionOrientation.top_left:
                                    gridBuilder.Grid.cells[x, y + 1].Connect(ConnectionOrientation.top_bottom_left);
                                    break;
                                case ConnectionOrientation.top_right:
                                    gridBuilder.Grid.cells[x, y + 1].Connect(ConnectionOrientation.top_bottom_right);
                                    break;
                                case ConnectionOrientation.top_left_right:
                                    gridBuilder.Grid.cells[x, y + 1].Connect(ConnectionOrientation.full);
                                    break;
                            }

                            switch (gridBuilder.Grid.cells[x, y].Connection)
                            {
                                case ConnectionOrientation.none:
                                    gridBuilder.Grid.cells[x, y].Connect(ConnectionOrientation.top);
                                    break;
                                case ConnectionOrientation.bottom:
                                    gridBuilder.Grid.cells[x, y].Connect(ConnectionOrientation.top_bottom);
                                    break;
                                case ConnectionOrientation.left:
                                    gridBuilder.Grid.cells[x, y].Connect(ConnectionOrientation.top_left);
                                    break;
                                case ConnectionOrientation.right:
                                    gridBuilder.Grid.cells[x, y].Connect(ConnectionOrientation.top_right);
                                    break;
                                case ConnectionOrientation.left_right:
                                    gridBuilder.Grid.cells[x, y].Connect(ConnectionOrientation.top_left_right);
                                    break;
                                case ConnectionOrientation.bottom_left:
                                    gridBuilder.Grid.cells[x, y].Connect(ConnectionOrientation.top_bottom_left);
                                    break;
                                case ConnectionOrientation.bottom_right:
                                    gridBuilder.Grid.cells[x, y].Connect(ConnectionOrientation.top_bottom_right);
                                    break;
                                case ConnectionOrientation.bottom_left_right:
                                    gridBuilder.Grid.cells[x, y].Connect(ConnectionOrientation.full);
                                    break;
                            }

                            UpdateBubbleConnectionList(gridBuilder.Grid.cells[x, y], gridBuilder.Grid.cells[x, y + 1]);
                        }
                    }
                }
            }

            UpdateImage();
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

            // Enable main bubble highlight
            mainBubble.EnableHighlight();

            // Set Initial Position
            mainBubble.SetPosition(gameSettings.MainBubbleSpawnPosition + Vector2Int.down);
            subBubble.SetPosition(gameSettings.SubBubbleSpawnPosition + Vector2Int.down);

            bubbleMovementController.SetBubbles(mainBubble, subBubble);

            // Next
            nextMainBubble = bubbleBuilder.Generate(gameSettings.NextMainBubblePosition);
            nextSubBubble = bubbleBuilder.Generate(gameSettings.NextSubBubblePosition);


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
                if (Time.time > nextInput)
                {
                    nextInput = Time.time + inputDelay;
                    if (Input.GetKey(KeyCode.LeftArrow))
                    {
                        bubbleMovementController.MoveLeft();
                    }

                    if (Input.GetKey(KeyCode.RightArrow))
                    {
                        bubbleMovementController.MoveRight();
                    }

                    if (Input.GetKey(KeyCode.DownArrow))
                    {
                        bubbleMovementController.MoveDown();
                        scoreController.AddPoints(10);
                    }
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