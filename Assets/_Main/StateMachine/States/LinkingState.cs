using System.Collections.Generic;
using System.Linq;

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
                    if (sessionVariables.gridBehaviour.Grid.IsOccupied(x, y))
                    {
                        emptyRow = false;
                        Bubble bubble = sessionVariables.gridBehaviour.Grid.GetBubble(x, y);
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

            for (int y = 0; y < sessionVariables.gameSettings.GridSize.y; y++)
            {
                emptyRow = true;

                for (int x = 0; x < sessionVariables.gameSettings.GridSize.x; x++)
                {
                    if (sessionVariables.gridBehaviour.Grid.IsOccupied(x, y))
                    {
                        emptyRow = false;

                        // Horizontal
                        if (x + 1 < sessionVariables.gridBehaviour.Grid.Size.x &&
                                    sessionVariables.gridBehaviour.Grid.IsOccupied(x + 1, y) &&
                                    sessionVariables.gridBehaviour.Grid.GetBubble(x, y).bubbleGroup == sessionVariables.gridBehaviour.Grid.GetBubble(x + 1, y).bubbleGroup)
                        {
                            if (sessionVariables.gridBehaviour.Grid.GetBubble(x, y).ConnectionController.Connection == ConnectionOrientation.left)
                                sessionVariables.gridBehaviour.Grid.GetBubble(x, y).ConnectionController.Connect(ConnectionOrientation.left_right);
                            else
                                sessionVariables.gridBehaviour.Grid.GetBubble(x, y).ConnectionController.Connect(ConnectionOrientation.right);

                            sessionVariables.gridBehaviour.Grid.GetBubble(x + 1, y).ConnectionController.Connect(ConnectionOrientation.left);

                            UpdateBubbleConnectionList(sessionVariables.gridBehaviour.Grid.GetBubble(x, y), sessionVariables.gridBehaviour.Grid.GetBubble(x + 1, y));
                        }
                    }
                }

                if (emptyRow)
                    break;
            }

            for (int y = 0; y < sessionVariables.gameSettings.GridSize.y; y++)
            {
                emptyRow = true;

                for (int x = 0; x < sessionVariables.gameSettings.GridSize.x; x++)
                {
                    if (sessionVariables.gridBehaviour.Grid.IsOccupied(x, y))
                    {
                        emptyRow = false;
                        // Vertical
                        if (y + 1 < sessionVariables.gridBehaviour.Grid.Size.y &&
                                    sessionVariables.gridBehaviour.Grid.IsOccupied(x, y + 1) &&
                                    sessionVariables.gridBehaviour.Grid.GetBubble(x, y).bubbleGroup == sessionVariables.gridBehaviour.Grid.GetBubble(x, y + 1).bubbleGroup)
                        {
                            switch (sessionVariables.gridBehaviour.Grid.GetBubble(x, y + 1).ConnectionController.Connection)
                            {
                                case ConnectionOrientation.none:
                                    sessionVariables.gridBehaviour.Grid.GetBubble(x, y + 1).ConnectionController.Connect(ConnectionOrientation.bottom);
                                    break;
                                case ConnectionOrientation.top:
                                    sessionVariables.gridBehaviour.Grid.GetBubble(x, y + 1).ConnectionController.Connect(ConnectionOrientation.top_bottom);
                                    break;
                                case ConnectionOrientation.left:
                                    sessionVariables.gridBehaviour.Grid.GetBubble(x, y + 1).ConnectionController.Connect(ConnectionOrientation.bottom_left);
                                    break;
                                case ConnectionOrientation.right:
                                    sessionVariables.gridBehaviour.Grid.GetBubble(x, y + 1).ConnectionController.Connect(ConnectionOrientation.bottom_right);
                                    break;
                                case ConnectionOrientation.left_right:
                                    sessionVariables.gridBehaviour.Grid.GetBubble(x, y + 1).ConnectionController.Connect(ConnectionOrientation.bottom_left_right);
                                    break;
                                case ConnectionOrientation.top_left:
                                    sessionVariables.gridBehaviour.Grid.GetBubble(x, y + 1).ConnectionController.Connect(ConnectionOrientation.top_bottom_left);
                                    break;
                                case ConnectionOrientation.top_right:
                                    sessionVariables.gridBehaviour.Grid.GetBubble(x, y + 1).ConnectionController.Connect(ConnectionOrientation.top_bottom_right);
                                    break;
                                case ConnectionOrientation.top_left_right:
                                    sessionVariables.gridBehaviour.Grid.GetBubble(x, y + 1).ConnectionController.Connect(ConnectionOrientation.full);
                                    break;
                            }

                            switch (sessionVariables.gridBehaviour.Grid.GetBubble(x, y).ConnectionController.Connection)
                            {
                                case ConnectionOrientation.none:
                                    sessionVariables.gridBehaviour.Grid.GetBubble(x, y).ConnectionController.Connect(ConnectionOrientation.top);
                                    break;
                                case ConnectionOrientation.bottom:
                                    sessionVariables.gridBehaviour.Grid.GetBubble(x, y).ConnectionController.Connect(ConnectionOrientation.top_bottom);
                                    break;
                                case ConnectionOrientation.left:
                                    sessionVariables.gridBehaviour.Grid.GetBubble(x, y).ConnectionController.Connect(ConnectionOrientation.top_left);
                                    break;
                                case ConnectionOrientation.right:
                                    sessionVariables.gridBehaviour.Grid.GetBubble(x, y).ConnectionController.Connect(ConnectionOrientation.top_right);
                                    break;
                                case ConnectionOrientation.left_right:
                                    sessionVariables.gridBehaviour.Grid.GetBubble(x, y).ConnectionController.Connect(ConnectionOrientation.top_left_right);
                                    break;
                                case ConnectionOrientation.bottom_left:
                                    sessionVariables.gridBehaviour.Grid.GetBubble(x, y).ConnectionController.Connect(ConnectionOrientation.top_bottom_left);
                                    break;
                                case ConnectionOrientation.bottom_right:
                                    sessionVariables.gridBehaviour.Grid.GetBubble(x, y).ConnectionController.Connect(ConnectionOrientation.top_bottom_right);
                                    break;
                                case ConnectionOrientation.bottom_left_right:
                                    sessionVariables.gridBehaviour.Grid.GetBubble(x, y).ConnectionController.Connect(ConnectionOrientation.full);
                                    break;
                            }

                            UpdateBubbleConnectionList(sessionVariables.gridBehaviour.Grid.GetBubble(x, y), sessionVariables.gridBehaviour.Grid.GetBubble(x, y + 1));
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
            for (int y = 0; y < sessionVariables.gameSettings.GridSize.y; y++)
            {
                emptyRow = true;

                for (int x = 0; x < sessionVariables.gameSettings.GridSize.x; x++)
                {
                    if (sessionVariables.gridBehaviour.Grid.IsOccupied(x, y))
                    {
                        emptyRow = false;

                        sessionVariables.gridBehaviour.Grid.GetBubble(x, y).UpdateGraphics();
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