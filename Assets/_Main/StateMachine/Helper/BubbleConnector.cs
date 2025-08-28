namespace EdwinGameDev.BubbleTeaMatch4
{
    public static class BubbleConnector
    {
        public static void ConnectHorizontal(Bubble left, Bubble right)
        {
            // left bubble orientation
            left.ConnectionController.Connect(
                left.ConnectionController.Connection == ConnectionOrientation.left
                    ? ConnectionOrientation.left_right
                    : ConnectionOrientation.right);

            // right bubble orientation
            right.ConnectionController.Connect(ConnectionOrientation.left);
        }

        public static void ConnectVertical(Bubble top, Bubble bottom)
        {
            // bottom bubble orientation
            switch (bottom.ConnectionController.Connection)
            {
                case ConnectionOrientation.none:
                    bottom.ConnectionController.Connect(ConnectionOrientation.bottom);
                    break;
                case ConnectionOrientation.top:
                    bottom.ConnectionController.Connect(ConnectionOrientation.top_bottom);
                    break;
                case ConnectionOrientation.left:
                    bottom.ConnectionController.Connect(ConnectionOrientation.bottom_left);
                    break;
                case ConnectionOrientation.right:
                    bottom.ConnectionController.Connect(ConnectionOrientation.bottom_right);
                    break;
                case ConnectionOrientation.left_right:
                    bottom.ConnectionController.Connect(ConnectionOrientation.bottom_left_right);
                    break;
                case ConnectionOrientation.top_left:
                    bottom.ConnectionController.Connect(ConnectionOrientation.top_bottom_left);
                    break;
                case ConnectionOrientation.top_right:
                    bottom.ConnectionController.Connect(ConnectionOrientation.top_bottom_right);
                    break;
                case ConnectionOrientation.top_left_right:
                    bottom.ConnectionController.Connect(ConnectionOrientation.full);
                    break;
            }

            // top bubble orientation
            switch (top.ConnectionController.Connection)
            {
                case ConnectionOrientation.none:
                    top.ConnectionController.Connect(ConnectionOrientation.top);
                    break;
                case ConnectionOrientation.bottom:
                    top.ConnectionController.Connect(ConnectionOrientation.top_bottom);
                    break;
                case ConnectionOrientation.left:
                    top.ConnectionController.Connect(ConnectionOrientation.top_left);
                    break;
                case ConnectionOrientation.right:
                    top.ConnectionController.Connect(ConnectionOrientation.top_right);
                    break;
                case ConnectionOrientation.left_right:
                    top.ConnectionController.Connect(ConnectionOrientation.top_left_right);
                    break;
                case ConnectionOrientation.bottom_left:
                    top.ConnectionController.Connect(ConnectionOrientation.top_bottom_left);
                    break;
                case ConnectionOrientation.bottom_right:
                    top.ConnectionController.Connect(ConnectionOrientation.top_bottom_right);
                    break;
                case ConnectionOrientation.bottom_left_right:
                    top.ConnectionController.Connect(ConnectionOrientation.full);
                    break;
            }
        }
    }
}