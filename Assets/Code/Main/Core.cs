using System;
using System.Collections;
using Code.Exceptions;
using Code.Main.Data;
using Code.Session;
using Code.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Code.Main
{
    public class Core : MonoBehaviour
    {
        public UIManager UiManager => uiManager;
        
        [SerializeField] private UIManager uiManager;
        [SerializeField] private SessionLoader sessionLoader;
        [SerializeField] private SettingsStorage settings;
        
        public static Core Instance { get; private set; }

        private Session _session;

        private IEnumerator Start()
        {
            if (Instance != null)
            {
                throw new CoreException("Core already created");
            }
                
            Instance = this;
            DontDestroyOnLoad(this);
            
            settings.InitializeSettings();
            
            var cachedYield = new WaitForSeconds(0.5f);
            
            uiManager.LoadingScreen.Show();
            uiManager.LoadingScreen.SetLoadingProgress(0.0f);

            yield return cachedYield;
            
            uiManager.Initialize();
            
            uiManager.LoadingScreen.SetLoadingProgress(0.3f);
            yield return cachedYield;
            
            uiManager.LoadingScreen.SetLoadingProgress(0.8f);
            yield return cachedYield;
            
            uiManager.LoadingScreen.SetLoadingProgress(1.0f);
            uiManager.LoadingScreen.Hide();

            ShowMainMenu();
            
            yield return null;
        }

        #region UI

        private void ShowMainMenu()
        {
            var mainMenu = uiManager.GetWindow<MainMenu>();
            mainMenu.Show();
        }
        
        private void HideMainMenu()
        {
            var mainMenu = uiManager.GetWindow<MainMenu>();
            mainMenu.Hide();
        }

        #endregion

        #region Session

        private void RegisterCurrentSession(Session session)
        {
            _session = session;
            
            uiManager.GetWindow<Shop>().OnBuildingSelected += _session.SetSelectedBuilding;
        }
        
        public void StartSession()
        {
            HideMainMenu();
            var context = GetSessionContext();
            
            StartCoroutine(GoToSessionAsync(context));
        }
        
        public void FinishSession()
        {
            var hud = uiManager.GetWindow<Hud>();
            hud.Hide();
            
            StartCoroutine(UnloadSession());
            ShowMainMenu();
        }

        private SessionContext GetSessionContext()
        {
            return new SessionContext
            {
                SceneName = "Session",
                FieldSizeX = settings.GameFieldX,
                FieldSizeY = settings.GameFieldY,
                ResourceData = settings.GetStartResourceData()
            };
        }

        private IEnumerator GoToSessionAsync(SessionContext context)
        {
            uiManager.LoadingScreen.Show();
            
            sessionLoader.StartSession(context, settings, RegisterCurrentSession);
            
            while (!sessionLoader.SessionReady)
            {
                yield return null;
            }
            
            uiManager.LoadingScreen.SetLoadingProgress(1.0f);
            uiManager.LoadingScreen.Hide();
            
            var hud = uiManager.GetWindow<Hud>();
            hud.Show();
        }

        private IEnumerator UnloadSession()
        {
            uiManager.GetWindow<Shop>().RemoveAllListeners();
            
            yield return SceneManager.UnloadSceneAsync(sessionLoader.SceneName);
            
            yield return Resources.UnloadUnusedAssets();
            
            _session = null;
            
            GC.Collect();
        }

        #endregion
    }
}
