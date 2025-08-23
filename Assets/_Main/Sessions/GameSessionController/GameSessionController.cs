using System;
using System.Collections.Generic;
using UnityEngine;

namespace EdwinGameDev.BubbleTeaMatch4
{
    public class GameSessionController : MonoBehaviour, IGameSessionController
    {
        // Config
        [SerializeField] private GameSettings gameSettings;
        [SerializeField] private GameObject matchScenarioPrefab;

        [SerializeField] private Camera singleplayerCamera;
        [SerializeField] private Camera vsCamera;

        // Layers
        [Range(1, 31)] [SerializeField] private int singleplayerLayer;
        [Range(1, 31)] [SerializeField] private int cpuLayer;

        // Session
        private Dictionary<int, GameSession> sessions = new();

        private PoolProvider poolProvider = new PoolProvider();
        private Dictionary<int, Pooling> bubblePool = new();
        private Dictionary<int, Pooling> evilBubblePool = new();
        IPool bubble;
        IPool evilBubble;

        private const int xDistanceBetweenPlayers = 50;

        private IGameMode currentGameMode;

        // Callbacks
        public event Action OnGameOver;
        public event Action OnWin;

        private void Start()
        {
            bubble = gameSettings.BubbleSettings.Prefab.GetComponent<IPool>();
            evilBubble = gameSettings.BubbleSettings.EvilPrefab.GetComponent<IPool>();
        }

        public void GameLoop()
        {
            foreach (var session in sessions)
            {
                if (session.Value.enabled)
                {
                    session.Value.Update();
                }
            }
        }

        public void StartGameMode<T>(T mode) where T : IGameMode
        {
            currentGameMode?.ExitMode();
            currentGameMode = mode;

            currentGameMode.StartMode();
        }

        public void StartSelfAttackMode()
        {
            StartGameMode(new SelfAttackMode(this));
        }

        public void StartSingleplayer()
        {
            StartGameMode(new SinglePlayerGameMode(this));
        }

        public void StartMultiplayer()
        {
            StartGameMode(new MultiplayerGameMode(this));
        }

        public void EnableSoloCamera()
        {
            singleplayerCamera.gameObject.SetActive(true);
            vsCamera.gameObject.SetActive(false);
        }

        public void EnableVSCamera()
        {
            vsCamera.gameObject.SetActive(true);
            singleplayerCamera.gameObject.SetActive(false);
        }

        private void SetLayerRecursively(GameObject obj, int layerIndex)
        {
            if (obj == null)
            {
                return;
            }

            obj.layer = layerIndex;

            foreach (Transform child in obj.transform)
            {
                if (child != null)
                {
                    SetLayerRecursively(child.gameObject, layerIndex);
                }
            }
        }

        private MatchScenario GenerateScenario(int layerIndex)
        {
            GameObject scenario = Instantiate(matchScenarioPrefab);
            scenario.SetActive(true);

            SetLayerRecursively(scenario, layerIndex);

            return scenario.GetComponent<MatchScenario>();
        }

        public GameSession GetOrCreateGameSession(int id, IInputProcessor inputProcessor)
        {
            if (sessions.TryGetValue(id, out var session))
            {
                return session;
            }

            int playerLayer = id == 0 ? singleplayerLayer : cpuLayer;

            MatchScenario matchScenario = GenerateScenario(playerLayer);

            bubblePool.Add(id, poolProvider.CreateBubblePool(bubble, "BubblePool", 50));
            evilBubblePool.Add(id, poolProvider.CreateBubblePool(evilBubble, "EvilBubblePool", 50));

            GameSession newSession = new GameSession(
                gameSettings,
                matchScenario,
                new Vector2Int(xDistanceBetweenPlayers * id, 0),
                inputProcessor,
                bubblePool[id],
                evilBubblePool[id]);

            sessions.Add(id, newSession);
            return newSession;
        }
    }
}