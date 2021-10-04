using System.Collections.Generic;

namespace EdwinGameDev.BubbleTeaMatch4
{
    public interface IConnectionController
    {
        List<Bubble> GetConnectionList();
        void SetConnectionList(List<Bubble> newConnections);
        void Connect(ConnectionOrientation newConnection);
        void Disconnect();
        ConnectionOrientation Connection { get ; }
        bool Matched { get;}

        void Reset();
    }
}