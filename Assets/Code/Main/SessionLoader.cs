using System;
using System.Collections;
using Code.Main.Data;
using Code.Session;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Code.Main
{
    public class SessionLoader : MonoBehaviour
    {
        public string SceneName => _sessionContext?.SceneName;
        public bool SessionReady => _sessionReady;
        
        private SessionContext _sessionContext;
        private SettingsStorage _settingsStorage;
        private Action<Session> _onSessionLoaded;
        private bool _sessionReady;
        
        public void StartSession(SessionContext context, SettingsStorage settingsStorage, Action<Session> onSessionLoaded)
        {
            _sessionReady = false;
            _sessionContext = context;
            _settingsStorage = settingsStorage;
            _onSessionLoaded = onSessionLoaded;
            StartCoroutine(Initialize());
        }
        
        private IEnumerator Initialize()
        {
            if (!SceneManager.GetSceneByName(SceneName).isLoaded)
            {
                var sceneLoadOperation = SceneManager.LoadSceneAsync(SceneName, LoadSceneMode.Additive);
                while (!sceneLoadOperation.isDone)
                {
                    yield return null;
                }

                SceneManager.SetActiveScene(SceneManager.GetSceneByName(SceneName));
            }

            var session = FindObjectOfType<Session>();
            _onSessionLoaded?.Invoke(session);
            session.InitializeSession(_sessionContext, _settingsStorage);
            
            _sessionReady = true;
            _onSessionLoaded = null;
        }
    }
}
