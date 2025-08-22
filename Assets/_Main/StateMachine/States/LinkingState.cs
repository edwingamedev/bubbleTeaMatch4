using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace EdwinGameDev.BubbleTeaMatch4
{
    public class LinkingState : IState
    {
        private SessionVariables sessionVariables;

        public LinkingState(SessionVariables sessionVariables)
        {
            this.sessionVariables = sessionVariables;
        }

        public void OnEnter()
        {
            ResetLinkStatus();
            LinkBubbles();
        }

        private void ResetLinkStatus()
        {
            bool emptyRow;
            for (int y = 0; y < sessionVariables.gameSettings.GridSize.y; y++)
            {
                emptyRow = true;

                for (int x = 0; x < sessionVariables.gameSettings.GridSize.x; x++)
                {
                    if (!sessionVariables.gridBehaviour.Grid.IsOccupied(x, y))
                    {
                        continue;
                    }

                    emptyRow = false;
                    Bubble bubble = sessionVariables.gridBehaviour.Grid.GetBubble(x, y);
                    bubble.ConnectionController.Disconnect();

                    var newConnection = new List<Bubble>();
                    newConnection.Add(bubble);

                    bubble.ConnectionController.SetConnectionList(newConnection);
                }

                if (emptyRow)
                {
                    break;
                }
            }
        }

        private void LinkBubbles()
        {
            bool emptyRow;

            Grid grid = sessionVariables.gridBehaviour.Grid;
            Vector2Int gridSize = sessionVariables.gameSettings.GridSize;

            for (int y = 0; y < gridSize.y; y++)
            {
                emptyRow = true;

                for (int x = 0; x < gridSize.x; x++)
                {
                    if (!grid.IsOccupied(x, y))
                    {
                        continue;
                    }

                    emptyRow = false;

                    // Horizontal
                    if (x + 1 >= grid.Size.x ||
                        !grid.IsOccupied(x + 1, y) ||
                        grid.GetBubble(x, y).bubbleGroup !=
                        grid.GetBubble(x + 1, y).bubbleGroup)
                    {
                        continue;
                    }

                    grid.GetBubble(x, y)
                        .ConnectionController
                        .Connect(grid.GetBubble(x, y)
                            .ConnectionController.Connection == ConnectionOrientation.left
                            ? ConnectionOrientation.left_right
                            : ConnectionOrientation.right);

                    grid.GetBubble(x + 1, y)
                        .ConnectionController
                        .Connect(ConnectionOrientation.left);

                    UpdateBubbleConnectionList(
                        grid.GetBubble(x, y),
                        grid.GetBubble(x + 1, y));
                }

                if (emptyRow)
                {
                    break;
                }
            }

            for (int y = 0; y < gridSize.y; y++)
            {
                for (int x = 0; x < gridSize.x; x++)
                {
                    if (!grid.IsOccupied(x, y))
                    {
                        continue;
                    }

                    // Vertical
                    if (y + 1 >= grid.Size.y ||
                        !grid.IsOccupied(x, y + 1) ||
                        grid.GetBubble(x, y).bubbleGroup !=
                        grid.GetBubble(x, y + 1).bubbleGroup)
                    {
                        continue;
                    }

                    switch (grid.GetBubble(x, y + 1).ConnectionController.Connection)
                    {
                        case ConnectionOrientation.none:
                            grid.GetBubble(x, y + 1)
                                .ConnectionController
                                .Connect(ConnectionOrientation.bottom);
                            break;
                        case ConnectionOrientation.top:
                            grid.GetBubble(x, y + 1)
                                .ConnectionController
                                .Connect(ConnectionOrientation.top_bottom);
                            break;
                        case ConnectionOrientation.left:
                            grid.GetBubble(x, y + 1)
                                .ConnectionController
                                .Connect(ConnectionOrientation.bottom_left);
                            break;
                        case ConnectionOrientation.right:
                            grid.GetBubble(x, y + 1)
                                .ConnectionController
                                .Connect(ConnectionOrientation.bottom_right);
                            break;
                        case ConnectionOrientation.left_right:
                            grid.GetBubble(x, y + 1)
                                .ConnectionController
                                .Connect(ConnectionOrientation.bottom_left_right);
                            break;
                        case ConnectionOrientation.top_left:
                            grid.GetBubble(x, y + 1)
                                .ConnectionController
                                .Connect(ConnectionOrientation.top_bottom_left);
                            break;
                        case ConnectionOrientation.top_right:
                            grid.GetBubble(x, y + 1)
                                .ConnectionController
                                .Connect(ConnectionOrientation.top_bottom_right);
                            break;
                        case ConnectionOrientation.top_left_right:
                            grid.GetBubble(x, y + 1)
                                .ConnectionController
                                .Connect(ConnectionOrientation.full);
                            break;
                    }

                    switch (grid.GetBubble(x, y).ConnectionController.Connection)
                    {
                        case ConnectionOrientation.none:
                            grid.GetBubble(x, y).ConnectionController
                                .Connect(ConnectionOrientation.top);
                            break;
                        case ConnectionOrientation.bottom:
                            grid.GetBubble(x, y)
                                .ConnectionController
                                .Connect(ConnectionOrientation.top_bottom);
                            break;
                        case ConnectionOrientation.left:
                            grid.GetBubble(x, y)
                                .ConnectionController
                                .Connect(ConnectionOrientation.top_left);
                            break;
                        case ConnectionOrientation.right:
                            grid.GetBubble(x, y)
                                .ConnectionController
                                .Connect(ConnectionOrientation.top_right);
                            break;
                        case ConnectionOrientation.left_right:
                            grid.GetBubble(x, y)
                                .ConnectionController
                                .Connect(ConnectionOrientation.top_left_right);
                            break;
                        case ConnectionOrientation.bottom_left:
                            grid.GetBubble(x, y)
                                .ConnectionController
                                .Connect(ConnectionOrientation.top_bottom_left);
                            break;
                        case ConnectionOrientation.bottom_right:
                            grid.GetBubble(x, y)
                                .ConnectionController
                                .Connect(ConnectionOrientation.top_bottom_right);
                            break;
                        case ConnectionOrientation.bottom_left_right:
                            grid.GetBubble(x, y)
                                .ConnectionController
                                .Connect(ConnectionOrientation.full);
                            break;
                    }

                    UpdateBubbleConnectionList(
                        grid.GetBubble(x, y),
                        grid.GetBubble(x, y + 1)
                    );
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
            for (int y = 0; y < sessionVariables.gameSettings.GridSize.y; y++)
            {
                emptyRow = true;

                for (int x = 0; x < sessionVariables.gameSettings.GridSize.x; x++)
                {
                    if (!sessionVariables.gridBehaviour.Grid.IsOccupied(x, y))
                    {
                        continue;
                    }

                    emptyRow = false;

                    sessionVariables.gridBehaviour.Grid.GetBubble(x, y).UpdateGraphics();
                }

                if (emptyRow)
                {
                    break;
                }
            }
        }

        public void OnExit()
        {
        }

        public void Tick()
        {
        }
    }
}