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
        [SerializeField] private ScoreController scoreController;

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
                sessions[0].InitializeSinglePlayer();            }
            else
            {
                GameSession session = new GameSession(gameSettings, scoreController, Vector2Int.zero, new KeyboardInputProcessor(), playerPool);

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
                GameSession session = new GameSession(gameSettings, scoreController, Vector2Int.zero, new KeyboardInputProcessor(), playerPool);

                session.InitializeSinglePlayer();
                sessions.Add(session);
            }

            if (sessions.Count > 1)
            {
                sessions[1].InitializeSinglePlayer();
            }
            else
            {
                GameSession session = new GameSession(gameSettings, scoreController, new Vector2Int(50,0), new AIInputProcessor(), cpuPool);

                session.InitializeSinglePlayer();
                sessions.Add(session);
            }

            // second Player
        }
    }
}