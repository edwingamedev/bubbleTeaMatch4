using System.Collections.Generic;
using System.Linq;

namespace EdwinGameDev.BubbleTeaMatch4
{
    public class LinkingState : IState
    {
        private GameBoard gameVariables;

        public LinkingState(GameBoard gameVariables)
        {
            this.gameVariables = gameVariables;
        }

        public void OnEnter()
        {
            ResetLinkStatus();
            LinkBubbles();
        }

        private void ResetLinkStatus()
        {
            bool emptyRow;
            for (int y = 0; y < gameVariables.gameSettings.GridSize.y; y++)
            {
                emptyRow = true;

                for (int x = 0; x < gameVariables.gameSettings.GridSize.x; x++)
                {
                    if (gameVariables.gridBehaviour.Grid.IsOccupied(x, y))
                    {
                        emptyRow = false;
                        Bubble bubble = gameVariables.gridBehaviour.Grid.GetBubble(x, y);
                        bubble.ConnectionController.Disconnect();

                        var newConnection = new List<Bubble>();
                        newConnection.Add(bubble);

                        bubble.ConnectionController.SetConnectionList(newConnection);
                    }
                }
                if (emptyRow)
                    break;
            }
        }

        private void LinkBubbles()
        {
            bool emptyRow;

            for (int y = 0; y < gameVariables.gameSettings.GridSize.y; y++)
            {
                emptyRow = true;

                for (int x = 0; x < gameVariables.gameSettings.GridSize.x; x++)
                {
                    if (gameVariables.gridBehaviour.Grid.IsOccupied(x, y))
                    {
                        emptyRow = false;

                        // Horizontal
                        if (x + 1 < gameVariables.gridBehaviour.Grid.Size.x &&
                                    gameVariables.gridBehaviour.Grid.IsOccupied(x + 1, y) &&
                                    gameVariables.gridBehaviour.Grid.GetBubble(x, y).bubbleGroup == gameVariables.gridBehaviour.Grid.GetBubble(x + 1, y).bubbleGroup)
                        {
                            if (gameVariables.gridBehaviour.Grid.GetBubble(x, y).ConnectionController.Connection == ConnectionOrientation.left)
                                gameVariables.gridBehaviour.Grid.GetBubble(x, y).ConnectionController.Connect(ConnectionOrientation.left_right);
                            else
                                gameVariables.gridBehaviour.Grid.GetBubble(x, y).ConnectionController.Connect(ConnectionOrientation.right);

                            gameVariables.gridBehaviour.Grid.GetBubble(x + 1, y).ConnectionController.Connect(ConnectionOrientation.left);

                            UpdateBubbleConnectionList(gameVariables.gridBehaviour.Grid.GetBubble(x, y), gameVariables.gridBehaviour.Grid.GetBubble(x + 1, y));
                        }
                    }
                }

                if (emptyRow)
                    break;
            }

            for (int y = 0; y < gameVariables.gameSettings.GridSize.y; y++)
            {
                emptyRow = true;

                for (int x = 0; x < gameVariables.gameSettings.GridSize.x; x++)
                {
                    if (gameVariables.gridBehaviour.Grid.IsOccupied(x, y))
                    {
                        emptyRow = false;
                        // Vertical
                        if (y + 1 < gameVariables.gridBehaviour.Grid.Size.y &&
                                    gameVariables.gridBehaviour.Grid.IsOccupied(x, y + 1) &&
                                    gameVariables.gridBehaviour.Grid.GetBubble(x, y).bubbleGroup == gameVariables.gridBehaviour.Grid.GetBubble(x, y + 1).bubbleGroup)
                        {
                            switch (gameVariables.gridBehaviour.Grid.GetBubble(x, y + 1).ConnectionController.Connection)
                            {
                                case ConnectionOrientation.none:
                                    gameVariables.gridBehaviour.Grid.GetBubble(x, y + 1).ConnectionController.Connect(ConnectionOrientation.bottom);
                                    break;
                                case ConnectionOrientation.top:
                                    gameVariables.gridBehaviour.Grid.GetBubble(x, y + 1).ConnectionController.Connect(ConnectionOrientation.top_bottom);
                                    break;
                                case ConnectionOrientation.left:
                                    gameVariables.gridBehaviour.Grid.GetBubble(x, y + 1).ConnectionController.Connect(ConnectionOrientation.bottom_left);
                                    break;
                                case ConnectionOrientation.right:
                                    gameVariables.gridBehaviour.Grid.GetBubble(x, y + 1).ConnectionController.Connect(ConnectionOrientation.bottom_right);
                                    break;
                                case ConnectionOrientation.left_right:
                                    gameVariables.gridBehaviour.Grid.GetBubble(x, y + 1).ConnectionController.Connect(ConnectionOrientation.bottom_left_right);
                                    break;
                                case ConnectionOrientation.top_left:
                                    gameVariables.gridBehaviour.Grid.GetBubble(x, y + 1).ConnectionController.Connect(ConnectionOrientation.top_bottom_left);
                                    break;
                                case ConnectionOrientation.top_right:
                                    gameVariables.gridBehaviour.Grid.GetBubble(x, y + 1).ConnectionController.Connect(ConnectionOrientation.top_bottom_right);
                                    break;
                                case ConnectionOrientation.top_left_right:
                                    gameVariables.gridBehaviour.Grid.GetBubble(x, y + 1).ConnectionController.Connect(ConnectionOrientation.full);
                                    break;
                            }

                            switch (gameVariables.gridBehaviour.Grid.GetBubble(x, y).ConnectionController.Connection)
                            {
                                case ConnectionOrientation.none:
                                    gameVariables.gridBehaviour.Grid.GetBubble(x, y).ConnectionController.Connect(ConnectionOrientation.top);
                                    break;
                                case ConnectionOrientation.bottom:
                                    gameVariables.gridBehaviour.Grid.GetBubble(x, y).ConnectionController.Connect(ConnectionOrientation.top_bottom);
                                    break;
                                case ConnectionOrientation.left:
                                    gameVariables.gridBehaviour.Grid.GetBubble(x, y).ConnectionController.Connect(ConnectionOrientation.top_left);
                                    break;
                                case ConnectionOrientation.right:
                                    gameVariables.gridBehaviour.Grid.GetBubble(x, y).ConnectionController.Connect(ConnectionOrientation.top_right);
                                    break;
                                case ConnectionOrientation.left_right:
                                    gameVariables.gridBehaviour.Grid.GetBubble(x, y).ConnectionController.Connect(ConnectionOrientation.top_left_right);
                                    break;
                                case ConnectionOrientation.bottom_left:
                                    gameVariables.gridBehaviour.Grid.GetBubble(x, y).ConnectionController.Connect(ConnectionOrientation.top_bottom_left);
                                    break;
                                case ConnectionOrientation.bottom_right:
                                    gameVariables.gridBehaviour.Grid.GetBubble(x, y).ConnectionController.Connect(ConnectionOrientation.top_bottom_right);
                                    break;
                                case ConnectionOrientation.bottom_left_right:
                                    gameVariables.gridBehaviour.Grid.GetBubble(x, y).ConnectionController.Connect(ConnectionOrientation.full);
                                    break;
                            }

                            UpdateBubbleConnectionList(gameVariables.gridBehaviour.Grid.GetBubble(x, y), gameVariables.gridBehaviour.Grid.GetBubble(x, y + 1));
                        }
                    }
                }
            }

            UpdateImage();
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

        private void UpdateImage()
        {
            bool emptyRow;
            for (int y = 0; y < gameVariables.gameSettings.GridSize.y; y++)
            {
                emptyRow = true;

                for (int x = 0; x < gameVariables.gameSettings.GridSize.x; x++)
                {
                    if (gameVariables.gridBehaviour.Grid.IsOccupied(x, y))
                    {
                        emptyRow = false;

                        gameVariables.gridBehaviour.Grid.GetBubble(x, y).UpdateGraphics();
                    }
                }
                if (emptyRow)
                    break;
            }
        }

        public void OnExit() { }

        public void Tick() { }
    }
}