using System.Collections.Generic;
using UnityEngine;

namespace EdwinGameDev.BubbleTeaMatch4
{
    [CreateAssetMenu(fileName = "BubbleSpriteConnections", menuName = "ScriptableObjects/BubbleSpriteConnections")]
    public class BubbleSpriteConnections : ScriptableObject
    {
        [SerializeField] private List<ConnectionOrientationsMapping> connectionOrientationsMapping;
        private Dictionary<ConnectionOrientation, Sprite> connectionMap = new Dictionary<ConnectionOrientation, Sprite>();


        public Sprite GetSprite(ConnectionOrientation ConnectionOrientations)
        {
            if (connectionMap.Count != connectionOrientationsMapping.Count)
                PrepareMap();

            return connectionMap[ConnectionOrientations];
        }

        private void PrepareMap()
        {
            connectionMap.Clear();

            foreach (var item in connectionOrientationsMapping)
            {
                connectionMap.Add(item.connectionOrientations, item.sprite);
            }
        }
    }
}