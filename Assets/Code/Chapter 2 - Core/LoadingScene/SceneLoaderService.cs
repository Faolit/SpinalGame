using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SpinalPlay
{
    public class SceneLoaderService : IService
    {
        private const float UNITY_PREPARED_SCENE = 0.9f;

        public void LoadScene(string sceneName, Action OnLoaded = null)
        {
            if (SceneManager.GetSceneByName(sceneName) != SceneManager.GetActiveScene())
            {
                AutonomCoroutineRunner.StartRoutine(LoadSceneRoutine(sceneName, OnLoaded));
                return;
            }
            OnLoaded?.Invoke();
        }

        private IEnumerator LoadSceneRoutine(string sceneName, Action OnLoaded)
        {
            AsyncOperation async = SceneManager.LoadSceneAsync(sceneName);

            async.allowSceneActivation = false;

            while (async.progress < UNITY_PREPARED_SCENE)
            {
                yield return null;
            }

            async.allowSceneActivation = true;

            while (SceneManager.GetActiveScene().name != sceneName)
            {
                yield return null;
            }

            OnLoaded?.Invoke();
        }
    }
}