using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace KG.Core
{
    public class SceneTransitions : MonoBehaviour
    {
        public System.Action OnLoadSceneFinishCallBack;

        bool m_bSceneLoading = false;
        public bool IsSceneLoading
        {
            get { return m_bSceneLoading; }
        }

        AsyncOperation m_asyncOperation;
        float m_currentProgress;

        public void ChangeScene(string _sceneName = null)
        {
            if (m_bSceneLoading || _sceneName == null)
                return;

            m_bSceneLoading = true;
            StartCoroutine(LoadSceneAsync(_sceneName));
        }

        IEnumerator LoadSceneAsync(string _sceneName)
        {
            m_asyncOperation = SceneManager.LoadSceneAsync(_sceneName);
            yield return m_asyncOperation;
            LoadSceneFinish();
            if (OnLoadSceneFinishCallBack != null)
                OnLoadSceneFinishCallBack.Invoke();
        }

        void LoadSceneFinish()
        {
            m_bSceneLoading = false;
            m_currentProgress = 0;
        }

        void Update()
        {
            if (m_bSceneLoading)
                m_currentProgress = (int)(m_asyncOperation.progress * 100);
        }
    }
}
