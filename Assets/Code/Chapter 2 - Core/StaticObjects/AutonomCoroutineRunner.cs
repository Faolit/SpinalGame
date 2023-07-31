using System.Collections;
using UnityEngine;

namespace SpinalPlay
{
    public class AutonomCoroutineRunner : MonoBehaviour
    {
        private static AutonomCoroutineRunner instance
        {
            get
            {
                if (_instance == null)
                {
                    GameObject go = new GameObject("AutonomCoroutineRunner");
                    _instance = go.AddComponent<AutonomCoroutineRunner>();
                    DontDestroyOnLoad(go);
                }
                return _instance;
            }
        }

        private static AutonomCoroutineRunner _instance;

        public static Coroutine StartRoutine(IEnumerator enumerator)
        {
            return instance.StartCoroutine(enumerator);
        }

        public static void StopRoutine(Coroutine coroutine)
        {
            instance.StopCoroutine(coroutine);
        }
    }
}