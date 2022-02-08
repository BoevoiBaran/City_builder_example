using System;
using Code.Main;
using UnityEngine;

namespace Code.UI
{
    public class Shop : MonoBehaviour
    {
        public event Action<string> OnBuildingSelected; 
        
        [SerializeField] private ShopView view;
        
        public void Show()
        {
            view.OnBuildingSelected += OnBuildingSelectHandler;
            view.OnCloseClicked += Hide;

            view.Show();
        }

        public void Hide()
        {
            view.OnBuildingSelected -= OnBuildingSelectHandler;
            view.OnCloseClicked -= Hide;

            view.Hide();
        }
        
        private void OnBuildingSelectHandler(string buildingId)
        {
            OnBuildingSelected?.Invoke(buildingId);
            
            Hide();
        }

        public void RemoveAllListeners()
        {
            OnBuildingSelected = null;
        }
    }
}
