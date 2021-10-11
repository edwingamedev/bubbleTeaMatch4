using System.Collections.Generic;
using UnityEngine;

namespace EdwinGameDev.BubbleTeaMatch4
{
    public class BubbleConnectionController : MonoBehaviour, IConnectionController
    {
        [SerializeField] private GameSettings gameSettings;
        [SerializeField] private ConnectionOrientation connection;
        public ConnectionOrientation Connection { get => connection; private set => connection = value; }

        [SerializeField] private List<Bubble> connectionList = new List<Bubble>();

        public bool Matched()
        {
            return GetConnectionList().Count >= gameSettings.BubblesAmountForMatch;
        } 

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
            Disconnect();
            connectionList.Clear();
        }
    }
}