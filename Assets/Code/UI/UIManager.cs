using System;
using System.Collections.Generic;
using Code.Extensions;
using Settings;
using UnityEngine;

namespace Code.UI
{
    // ReSharper disable InconsistentNaming
    public class UIManager : MonoBehaviour
    {
        public LoadingScreen LoadingScreen => loadingScreen;

        [SerializeField] private UILayouts layouts;
        [SerializeField] private LoadingScreen loadingScreen;

        #region Registred windows
        
        private readonly Dictionary<Type, MonoBehaviour> _registeredWindows = new Dictionary<Type, MonoBehaviour>();
        
        #endregion

        public void Initialize()
        {
            RegisterWindows();
        }
        
        public T GetWindow<T>() where T : MonoBehaviour
        {
            var type = typeof(T);
            
            if (_registeredWindows.ContainsKey(type))
            {
                return (T) _registeredWindows[type];
            }

            return null;
        }

        private void RegisterWindows()
        {
            var mainMenu =
                layouts.MainMenuRoot.AddNestedObjectFromResources($"{Constants.UiAssetPath}Menu").GetComponent<MainMenu>();
            mainMenu.Hide();
            
            _registeredWindows[typeof(MainMenu)] = mainMenu;

            var hud =
                layouts.HudRoot.AddNestedObjectFromResources($"{Constants.UiAssetPath}Hud").GetComponent<Hud>();
            hud.Hide();
            
            _registeredWindows[typeof(Hud)] = hud;
            
            
            var shop =
                layouts.ShopRoot.AddNestedObjectFromResources($"{Constants.UiAssetPath}Shop").GetComponent<Shop>();
            shop.Hide();
            
            _registeredWindows[typeof(Shop)] = shop;
        }
    }
}
