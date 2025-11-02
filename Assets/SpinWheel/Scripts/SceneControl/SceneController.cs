using System;
using System.Collections;
using System.Collections.Generic;
using SpinWheel.Scripts.Utility;
using SpinWheel.Scripts.Utility.Event;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SpinWheel.Scripts.SceneControl
{
    public class SceneController : MonoSingleton<SceneController>
    {
        private const string OverlaySceneName = "Overlay";
        private const string GameSceneName = "SpinGame";

        private void Awake()
        {
            InitializeSingleton();
            SceneManager.LoadScene(OverlaySceneName, LoadSceneMode.Additive);
        }

        public void OpenGameScene()
        {
            StartCoroutine(OpenSceneCoroutine(GameSceneName));
            
        }

        private IEnumerator OpenSceneCoroutine(string sceneName)
        {
            float lastProgress = 0f;

            AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
            while (!asyncOperation.isDone)
            {
                if (asyncOperation.progress - lastProgress > 0.1f)
                {
                    lastProgress = asyncOperation.progress;
                }

                yield return null;
            }
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(GameSceneName));
            yield return null;
        }
    }
}