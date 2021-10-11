using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace EdwinGameDev.BubbleTeaMatch4
{
    public class GameSessionController : MonoBehaviour
    {
        private const int xDistanceBetweenPlayers = 50;
        [SerializeField] private Camera singleplayerCamera;
        [SerializeField] private Camera vsCamera;

        [SerializeField] private GameSettings gameSettings;
        [SerializeField] private GameObject matchScenarioPrefab;
        [Range(1, 31)]
        [SerializeField] private int playerLayer;
        [Range(1, 31)]
        [SerializeField] private int cpuLayer;

        private List<GameSession> sessions = new List<GameSession>();
        private PoolProvider poolProvider = new PoolProvider();
        private List<Pooling> bubblePool = new List<Pooling>();
        private List<Pooling> evilBubblePool = new List<Pooling>();
        private IInputProcessor playerInput = new KeyboardInputProcessor();

        public Action OnGameOver;
        public Action OnWin;

        private void Start()
        {
            var bubble = gameSettings.BubbleSettings.Prefab.GetComponent<IPool>();
            var evilBubble = gameSettings.BubbleSettings.EvilPrefab.GetComponent<IPool>();

            bubblePool.Add(poolProvider.CreateBubblePool(bubble, "BubblePool", 50));
            bubblePool.Add(poolProvider.CreateBubblePool(bubble, "BubblePool", 50));

            evilBubblePool.Add(poolProvider.CreateBubblePool(evilBubble, "EvilBubblePool", 50));
            evilBubblePool.Add(poolProvider.CreateBubblePool(evilBubble, "EvilBubblePool", 50));
        }

        public void GameLoop()
        {
            foreach (var session in sessions)
            {
                if (session.enabled)
                    session.Update();
            }
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

        public void StartSelfAttackMode()
        {
            EnableSoloCamera();

            // First Player
            if (sessions.Count <= 0)
            {
                MatchScenario matchScenario = GenerateScenario(playerLayer);

                GameSession session = new GameSession(gameSettings, matchScenario, Vector2Int.zero, playerInput, bubblePool[0], evilBubblePool[0]);

                sessions.Add(session);
            }

            DisableSessions();
            sessions[0].enabled = true;

            sessions[0].OnCombo = sessions[0].EnemyAttack;
            sessions[0].OnGameOver = OnGameOver;
            sessions[0].InitializeSinglePlayer();
        }

        private void DisableSessions()
        {
            foreach (var session in sessions)
            {
                session.enabled = false;
            }
        }

        public void StartSingleplayer()
        {
            EnableSoloCamera();

            // First Player
            if (sessions.Count <= 0)
            {

                MatchScenario matchScenario = GenerateScenario(playerLayer);

                GameSession session = new GameSession(gameSettings, matchScenario, Vector2Int.zero, playerInput, bubblePool[0], evilBubblePool[0]);

                sessions.Add(session);
            }

            DisableSessions();
            sessions[0].enabled = true;
            sessions[0].OnCombo = null;
            sessions[0].OnGameOver = OnGameOver;
            sessions[0].InitializeSinglePlayer();
        }

        public void StartMultiplayer()
        {
            EnableVSCamera();

            // First Player
            if (sessions.Count <= 0)
            {
                MatchScenario matchScenario = GenerateScenario(playerLayer);

                GameSession session = new GameSession(gameSettings, matchScenario, Vector2Int.zero, playerInput, bubblePool[0], evilBubblePool[0]);

                sessions.Add(session);
            }

            // second Player
            if (sessions.Count <= 1)
            {
                MatchScenario matchScenario = GenerateScenario(cpuLayer);
                GameSession session = new GameSession(gameSettings, matchScenario, new Vector2Int(xDistanceBetweenPlayers, 0), new AIInputProcessor(), bubblePool[1], evilBubblePool[1]);

                sessions.Add(session);
            }

            DisableSessions();

            sessions[0].enabled = true;
            sessions[0].OnGameOver = OnGameOver;
            sessions[0].OnCombo = sessions[1].EnemyAttack;

            sessions[1].enabled = true;
            sessions[1].OnCombo = sessions[0].EnemyAttack;
            sessions[1].OnGameOver = OnWin;

            for (int i = 0; i < sessions.Count; i++)
            {
                sessions[i].InitializeSinglePlayer();
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