using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace EdwinGameDev.BubbleTeaMatch4
{
    public class GameSessionController : MonoBehaviour
    {        
        [SerializeField] private Camera singleplayerCamera;
        [SerializeField] private Camera vsCamera;

        [SerializeField] private GameSettings gameSettings;
        [SerializeField] private GameObject matchScenarioPrefab;

        [SerializeField] private int playerLayer;
        [SerializeField] private int cpuLayer;

        [SerializeField] private List<GameSession> sessions = new List<GameSession>();
        private Pooling playerPool;
        private Pooling cpuPool;
        public Action OnGameOver;

        private void Start()
        {
            CreateBubblePool();
        }

        public void GameLoop()
        {
            foreach (var session in sessions)
            {
                session.Update();
            }
        }

        private void CreateBubblePool()
        {
            var go = new GameObject();
            go.name = "BubblePool";
            playerPool = new Pooling(go.transform);
            playerPool.CreatePool(gameSettings.BubbleSettings.Prefab.GetComponent<IPool>(), 50);

            var go2 = new GameObject();
            go2.name = "BubblePool";
            cpuPool = new Pooling(go2.transform);
            cpuPool.CreatePool(gameSettings.BubbleSettings.Prefab.GetComponent<IPool>(), 50);
        }

        private void EnableVSCamera()
        {
            vsCamera.gameObject.SetActive(true);
            singleplayerCamera.gameObject.SetActive(false);
        }

        private void EnableSoloCamera()
        {
            singleplayerCamera.gameObject.SetActive(true);
            vsCamera.gameObject.SetActive(false);
        }

        public void StartSingleplayer()
        {
            EnableSoloCamera();

            // First Player
            if (sessions.Count > 0)
            {
                sessions[0].InitializeSinglePlayer();            
            }
            else
            {
                MatchScenario matchScenario = GenerateScenario(playerLayer);

                GameSession session = new GameSession(gameSettings, matchScenario, Vector2Int.zero, new KeyboardInputProcessor(), playerPool);

                session.InitializeSinglePlayer();
                sessions.Add(session);
            }
        }

        public void StartMultiplayer()
        {
            EnableVSCamera();

            // First Player
            if (sessions.Count > 0)
            {
                sessions[0].InitializeSinglePlayer();
            }
            else
            {
                MatchScenario matchScenario = GenerateScenario(playerLayer);

                GameSession session = new GameSession(gameSettings, matchScenario, Vector2Int.zero, new KeyboardInputProcessor(), playerPool);

                session.InitializeSinglePlayer();
                sessions.Add(session);
            }

            // second Player
            if (sessions.Count > 1)
            {
                sessions[1].InitializeSinglePlayer();
            }
            else
            {
                MatchScenario matchScenario = GenerateScenario(cpuLayer);
                GameSession session = new GameSession(gameSettings, matchScenario, new Vector2Int(50,0), new AIInputProcessor(), cpuPool);

                session.InitializeSinglePlayer();
                sessions.Add(session);
            }
        }

        private void SetLayerRecursively(GameObject obj, int layerIndex)
        {
            if (null == obj)
                return;

            obj.layer = layerIndex;

            foreach (Transform child in obj.transform)
            {
                if (null == child)
                    continue;

                SetLayerRecursively(child.gameObject, layerIndex);
            }
        }

        private MatchScenario GenerateScenario(int layerIndex)
        {
            GameObject scenario = Instantiate(matchScenarioPrefab);
            scenario.SetActive(true);

            SetLayerRecursively(scenario, layerIndex);

            return scenario.GetComponent<MatchScenario>();
        }
    }
}