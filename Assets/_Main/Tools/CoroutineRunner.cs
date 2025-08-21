using UnityEngine;
using System.Collections;

namespace EdwinGameDev.BubbleTeaMatch4
{
    public class CoroutineRunner : MonoBehaviour
    {
        private static CoroutineRunner _instance;

        public static CoroutineRunner Instance
        {
            get
            {
                if (_instance)
                {
                    return _instance;
                }

                GameObject go = new GameObject("CoroutineRunner");
                DontDestroyOnLoad(go); // keep across scenes
                _instance = go.AddComponent<CoroutineRunner>();

                return _instance;
            }
        }

        public Coroutine Run(IEnumerator routine)
        {
            return StartCoroutine(routine);
        }

        public void Stop(Coroutine coroutine)
        {
            if (coroutine != null)
                StopCoroutine(coroutine);
        }
    }
}