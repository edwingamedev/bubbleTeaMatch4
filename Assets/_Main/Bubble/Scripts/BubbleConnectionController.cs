using System.Collections.Generic;

namespace EdwinGameDev.BubbleTeaMatch4
{
    public class BubbleConnectionController : IConnectionController
    {        
        public ConnectionOrientation Connection { get; private set; }

        private List<Bubble> connectionList;
        private GameSettings gameSettings;

        public BubbleConnectionController(GameSettings gameSettings)
        {
            this.gameSettings = gameSettings;

            Connection = ConnectionOrientation.none;
            connectionList = new List<Bubble>();
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
            Connection = ConnectionOrientation.none;
        }

        public void Connect(ConnectionOrientation newConnection)
        {
            Connection = newConnection;
        }

        public void Reset()
        {
            Connection = ConnectionOrientation.none;
            connectionList = new List<Bubble>();
        }
    }
}