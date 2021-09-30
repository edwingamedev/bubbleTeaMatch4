using System.Collections.Generic;

namespace EdwinGameDev.BubbleTeaMatch4
{
    public class BubbleConnectionController : IConnectionController
    {
        private ConnectionOrientation connection = ConnectionOrientation.none;
        public ConnectionOrientation Connection { get => connection; }

        private List<Bubble> connectionList = new List<Bubble>();
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