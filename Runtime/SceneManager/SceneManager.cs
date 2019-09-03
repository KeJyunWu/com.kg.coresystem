using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace KG.Core
{
    public class SceneManager : MonoBehaviour
    {

        private static SceneManager m_instance;
        public static SceneManager instance
        {
            get { return m_instance; }
        }

        public System.Action OnLoadSceneFinishCallBack;

        bool m_bSceneLoading = false;
        AsyncOperation m_asyncOperation;
        float m_currentProgress;

        public void ChangeScene(string _sceneName)
        {
            if (m_bSceneLoading)
                return;

            m_bSceneLoading = true;
            StartCoroutine(LoadSceneAsync(_sceneName));
        }

        IEnumerator LoadSceneAsync(string _sceneName)
        {
            m_asyncOperation = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(_sceneName);
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

        void Awake()
        {
            m_instance = this;
            DontDestroyOnLoad(this.gameObject);
        }

        void Update()
        {
            if (m_bSceneLoading)
                m_currentProgress = (int)(m_asyncOperation.progress * 100);
        }
    }
}
