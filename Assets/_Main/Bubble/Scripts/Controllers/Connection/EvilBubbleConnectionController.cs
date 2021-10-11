using System.Collections.Generic;
using UnityEngine;

namespace EdwinGameDev.BubbleTeaMatch4
{
    public class EvilBubbleConnectionController : MonoBehaviour, IConnectionController
    {        
        public ConnectionOrientation Connection => ConnectionOrientation.none;

        private List<Bubble> connectionList = new List<Bubble>();

        public bool Matched()
        {
            return false;
        }

        public List<Bubble> GetConnectionList()
        {
            return connectionList;
        }

        public void SetConnectionList(List<Bubble> newConnections) { }
        public void Disconnect() { }
        public void Connect(ConnectionOrientation newConnection) { }
        public void Reset() { }
    }
}