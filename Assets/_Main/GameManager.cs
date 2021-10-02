using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace EdwinGameDev.BubbleTeaMatch4
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private GameSettings gameSettings;
        [SerializeField] private ScoreController scoreController;

        private GameState gameState = GameState.Initialize;

        bool isArranging = false;
        bool isPoping = false;

        private GameState previousGameState;

        // Refactor
        private IInputController inputController;
        public GridBehaviour gridBehaviour;
        private BubbleSpawner bubbleSpawner;

        private float dropRate = 1f;
        private float nextDrop;

        // Start is called before the first frame update
        void Start()
        {
            gameState = GameState.Initialize;
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                previousGameState = gameState;

                if (gameState != GameState.Pause)
                    gameState = GameState.Pause;
                else
                    gameState = previousGameState;
            }

            switch (gameState)
            {
                case GameState.Initialize:
                    Initialize();

                    // Start Game
                    gameState = GameState.Creating;
                    break;
                case GameState.Creating:
                    //Spawn new bubble set
                    bubbleSpawner.SpawnNewBubbleSet();

                    // Update controlled bubbles
                    inputController.SetBubbles(bubbleSpawner.CurrentSet);

                    // Start Game
                    gameState = GameState.Playing;
                    break;
                case GameState.Playing:
                    BubbleDrop();

                    // Player Movement
                    if (gridBehaviour.ValidateBubbleMovement(bubbleSpawner.CurrentSet, () => OnReachedBottom()))
                        inputController.CheckInputs();
                    break;
                case GameState.Arrange:
                    if (!isArranging)
                        StartCoroutine(ArrangeBubbles());
                    break;
                case GameState.Linking:
                    LinkBubbles();

                    gameState = GameState.Combo;
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

        private void Initialize()
        {
            bubbleSpawner = new BubbleSpawner(gameSettings);
            gridBehaviour = new GridBehaviour(gameSettings);

            inputController = new BubbleInputController(gridBehaviour.Grid,
                                                        new KeyboardInputProcessor(),
                                                        () => scoreController.AddPoints(10)
                                                        );
        }

        private IEnumerator PopAndFillGap()
        {
            yield return new WaitForSeconds(.5f);

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
                    if (gridBehaviour.Grid.cells[x, y] != null)
                    {
                        emptyRow = false;
                        gridBehaviour.Grid.cells[x, y].UpdateGraphics();
                    }
                }
                if (emptyRow)
                    break;
            }
        }

        private void UpdateBubbleConnectionList(Bubble bubbleA, Bubble bubbleB)
        {
            List<Bubble> bubbleAList = bubbleA.ConnectionController.GetConnectionList();
            if (!bubbleAList.Contains(bubbleB))
            {
                bubbleAList.Add(bubbleB);
            }

            List<Bubble> bubbleBList = bubbleB.ConnectionController.GetConnectionList();
            if (!bubbleBList.Contains(bubbleA))
            {
                bubbleBList.Add(bubbleA);
            }

            List<Bubble> bubbleCList = bubbleAList.Union(bubbleBList).ToList();

            for (int i = 0; i < bubbleAList.Count; i++)
            {
                bubbleAList[i].ConnectionController.SetConnectionList(bubbleCList);
            }
            for (int i = 0; i < bubbleBList.Count; i++)
            {
                bubbleBList[i].ConnectionController.SetConnectionList(bubbleCList);
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
                    if (gridBehaviour.Grid.cells[x, y] != null)
                    {
                        emptyRow = false;

                        if (gridBehaviour.Grid.cells[x, y].ConnectionController.GetConnectionList().Count >= 4)
                        {
                            hasConnection = true;
                            gridBehaviour.Grid.cells[x, y].ConnectionController.Matched = true;
                            gridBehaviour.Grid.cells[x, y].GraphicsController.PopAnimation();
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
                    if (gridBehaviour.Grid.cells[x, y] != null)
                    {
                        if (gridBehaviour.Grid.cells[x, y].ConnectionController.Matched)
                        {
                            Destroy(gridBehaviour.Grid.cells[x, y].gameObject);
                            gridBehaviour.Grid.cells[x, y] = null;

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
                    if (gridBehaviour.Grid.cells[x, y] != null)
                    {
                        emptyRow = false;

                        // Horizontal
                        if (x + 1 < gridBehaviour.Grid.cells.GetLength(0) && gridBehaviour.Grid.cells[x + 1, y] != null && gridBehaviour.Grid.cells[x, y].BubbleGroup == gridBehaviour.Grid.cells[x + 1, y].BubbleGroup)
                        {
                            if (gridBehaviour.Grid.cells[x, y].ConnectionController.Connection == ConnectionOrientation.left)
                                gridBehaviour.Grid.cells[x, y].ConnectionController.Connect(ConnectionOrientation.left_right);
                            else
                                gridBehaviour.Grid.cells[x, y].ConnectionController.Connect(ConnectionOrientation.right);

                            gridBehaviour.Grid.cells[x + 1, y].ConnectionController.Connect(ConnectionOrientation.left);

                            UpdateBubbleConnectionList(gridBehaviour.Grid.cells[x, y], gridBehaviour.Grid.cells[x + 1, y]);
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
                    if (gridBehaviour.Grid.cells[x, y] != null)
                    {
                        emptyRow = false;
                        // Vertical
                        if (y + 1 < gridBehaviour.Grid.cells.GetLength(1) && gridBehaviour.Grid.cells[x, y + 1] != null && gridBehaviour.Grid.cells[x, y].BubbleGroup == gridBehaviour.Grid.cells[x, y + 1].BubbleGroup)
                        {
                            switch (gridBehaviour.Grid.cells[x, y + 1].ConnectionController.Connection)
                            {
                                case ConnectionOrientation.none:
                                    gridBehaviour.Grid.cells[x, y + 1].ConnectionController.Connect(ConnectionOrientation.bottom);
                                    break;
                                case ConnectionOrientation.top:
                                    gridBehaviour.Grid.cells[x, y + 1].ConnectionController.Connect(ConnectionOrientation.top_bottom);
                                    break;
                                case ConnectionOrientation.left:
                                    gridBehaviour.Grid.cells[x, y + 1].ConnectionController.Connect(ConnectionOrientation.bottom_left);
                                    break;
                                case ConnectionOrientation.right:
                                    gridBehaviour.Grid.cells[x, y + 1].ConnectionController.Connect(ConnectionOrientation.bottom_right);
                                    break;
                                case ConnectionOrientation.left_right:
                                    gridBehaviour.Grid.cells[x, y + 1].ConnectionController.Connect(ConnectionOrientation.bottom_left_right);
                                    break;
                                case ConnectionOrientation.top_left:
                                    gridBehaviour.Grid.cells[x, y + 1].ConnectionController.Connect(ConnectionOrientation.top_bottom_left);
                                    break;
                                case ConnectionOrientation.top_right:
                                    gridBehaviour.Grid.cells[x, y + 1].ConnectionController.Connect(ConnectionOrientation.top_bottom_right);
                                    break;
                                case ConnectionOrientation.top_left_right:
                                    gridBehaviour.Grid.cells[x, y + 1].ConnectionController.Connect(ConnectionOrientation.full);
                                    break;
                            }

                            switch (gridBehaviour.Grid.cells[x, y].ConnectionController.Connection)
                            {
                                case ConnectionOrientation.none:
                                    gridBehaviour.Grid.cells[x, y].ConnectionController.Connect(ConnectionOrientation.top);
                                    break;
                                case ConnectionOrientation.bottom:
                                    gridBehaviour.Grid.cells[x, y].ConnectionController.Connect(ConnectionOrientation.top_bottom);
                                    break;
                                case ConnectionOrientation.left:
                                    gridBehaviour.Grid.cells[x, y].ConnectionController.Connect(ConnectionOrientation.top_left);
                                    break;
                                case ConnectionOrientation.right:
                                    gridBehaviour.Grid.cells[x, y].ConnectionController.Connect(ConnectionOrientation.top_right);
                                    break;
                                case ConnectionOrientation.left_right:
                                    gridBehaviour.Grid.cells[x, y].ConnectionController.Connect(ConnectionOrientation.top_left_right);
                                    break;
                                case ConnectionOrientation.bottom_left:
                                    gridBehaviour.Grid.cells[x, y].ConnectionController.Connect(ConnectionOrientation.top_bottom_left);
                                    break;
                                case ConnectionOrientation.bottom_right:
                                    gridBehaviour.Grid.cells[x, y].ConnectionController.Connect(ConnectionOrientation.top_bottom_right);
                                    break;
                                case ConnectionOrientation.bottom_left_right:
                                    gridBehaviour.Grid.cells[x, y].ConnectionController.Connect(ConnectionOrientation.full);
                                    break;
                            }

                            UpdateBubbleConnectionList(gridBehaviour.Grid.cells[x, y], gridBehaviour.Grid.cells[x, y + 1]);
                        }
                    }
                }
            }

            UpdateImage();
        }

        public void OnReachedBottom()
        {
            if (CheckGameOver()) // bubble is greater than grid height
                SetGameOver();
            else // bubble has reached bottom
            {
                nextDrop = 0;
                gameState = GameState.Arrange;
            }
        }

        private void BubbleDrop()
        {
            if (Time.time > nextDrop)
            {
                nextDrop = Time.time + dropRate;

                // Validate Bubble Drop
                if (gridBehaviour.ValidateBubbleMovement(bubbleSpawner.CurrentSet, () => OnReachedBottom()))
                {
                    // Move down
                    inputController.ValidateAndMoveDown();
                }
            }
        }

        private bool CheckGameOver()
        {
            return gridBehaviour.InBounds(bubbleSpawner.CurrentSet);
        }

        private void SetGameOver()
        {
            gameState = GameState.Pause;
            Debug.Log("GAME OVER");
        }

        private IEnumerator ArrangeBubbles()
        {
            isArranging = true;

            for (int y = 1; y < gameSettings.GridSize.y; y++)
            {
                for (int x = 0; x < gameSettings.GridSize.x; x++)
                {
                    if (gridBehaviour.Grid.cells[x, y] != null)
                    {
                        if (gridBehaviour.Grid.cells[x, y - 1] == null)
                        {
                            Bubble bubble = gridBehaviour.Grid.cells[x, y];
                            bubble.MovementController.MoveDirection(Vector2Int.down);
                            gridBehaviour.Grid.cells[x, y - 1] = gridBehaviour.Grid.cells[x, y];
                            gridBehaviour.Grid.cells[x, y] = null;
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
    }
}