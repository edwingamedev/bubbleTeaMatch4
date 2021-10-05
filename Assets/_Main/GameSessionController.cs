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
        private Pooling bubblePool;
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
            bubblePool = new Pooling(go.transform);
            bubblePool.CreatePool(gameSettings.BubbleSettings.Prefab.GetComponent<IPool>(), 50);
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
                GameSession session = new GameSession(gameSettings, scoreController, Vector2.zero, new KeyboardInputProcessor(), bubblePool);

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
                GameSession session = new GameSession(gameSettings, scoreController, Vector2.zero, new KeyboardInputProcessor(), bubblePool);

                session.InitializeSinglePlayer();
                sessions.Add(session);
            }

            if (sessions.Count > 1)
            {
                sessions[1].InitializeSinglePlayer();
            }
            else
            {
                GameSession session = new GameSession(gameSettings, scoreController, new Vector2(50,0), new AIInputProcessor(), bubblePool);

                session.InitializeSinglePlayer();
                sessions.Add(session);
            }

            // second Player
        }
    }
}