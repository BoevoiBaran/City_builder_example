using System;
using System.Text;
using Code.Main;
using Code.Main.Data;
using UnityEngine;

namespace Code.UI
{
    public class Hud : MonoBehaviour
    {
        [SerializeField] private HudView view;
        
        private readonly StringBuilder _sb = new StringBuilder();

        public void Show()
        {
            view.Show();
        }

        public void Hide()
        {
            view.Hide();
        }
        
        public void SetResourceLabel(ResourceData resources)
        {
            for (int i = 0; i < resources.Data.Count; i++)
            {
                var (type, count, limit) = resources.Data[i];

                _sb.Append($"{type} : {count}/{limit}");
                _sb.Append("\n");
            }
            
            view.SetResourceLabel(_sb.ToString());
            _sb.Remove(0, _sb.Length);
        }
        
        public void SetSelectedBuildingLabel(string buildingId)
        {
            view.SetSelectedBuildingLabel(buildingId);
        }
        
        public void ResetSelectedBuildingLevel()
        {
            view.SetSelectedBuildingLabel(String.Empty);
        }

        private void OnEnable()
        {
            view.OnShopOpened += OpenShop;
            view.OnGameRestarted += RestartGame;
        }

        private void OnDisable()
        {
            view.OnShopOpened -= OpenShop;
            view.OnGameRestarted -= RestartGame;
        }

        private void OpenShop()
        {
            Core.Instance.UiManager.GetWindow<Shop>().Show();
        }

        private void RestartGame()
        {
            Core.Instance.FinishSession();
        }
    }
}

