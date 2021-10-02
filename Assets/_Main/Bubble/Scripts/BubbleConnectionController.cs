using System.Collections.Generic;

namespace EdwinGameDev.BubbleTeaMatch4
{
    public class BubbleConnectionController : IConnectionController
    {
        private ConnectionOrientation connection;
        public ConnectionOrientation Connection  => connection;

        private List<Bubble> connectionList = new List<Bubble>();

        public BubbleConnectionController()
        {
            connection = ConnectionOrientation.none;
        }

        public bool Matched { get; set; }

        public List<Bubble> GetConnectionList()
        {
            return connectionList;
        }

        public void SetConnectionList(List<Bubble> newConnections)
        {
            this.connectionList = newConnections;
        }

        public void Disconnect()
        {
            connection = ConnectionOrientation.none;
        }

        public void Connect(ConnectionOrientation newConnection)
        {
            connection = newConnection;
        }
    }
}