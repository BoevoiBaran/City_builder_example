using System;
using Code.Extensions;
using UnityEngine;
using UnityEngine.UI;

namespace Code.UI
{
    public class HudView : MonoBehaviour
    {
        public event Action OnShopOpened;
        public event Action OnGameRestarted;
        
        [SerializeField] private Button shopBtn;
        [SerializeField] private Button restartBtn;
        [SerializeField] private Text resourceLabel;
        [SerializeField] private Text selectedBuildingLabel;

        public void Show()
        {
            gameObject.SetSafeActive(true);
        }

        public void Hide()
        {
            gameObject.SetSafeActive(false);
        }

        public void SetResourceLabel(string value)
        {
            resourceLabel.text = value;
        }
        
        public void SetSelectedBuildingLabel(string value)
        {
            selectedBuildingLabel.text = value;
        }

        private void OnEnable()
        {
            shopBtn.onClick.AddListener(OnShopOpenedClickHandler);
            restartBtn.onClick.AddListener(OnRestartClickHandler);
        }

        private void OnDisable()
        {
            shopBtn.onClick.RemoveAllListeners();
            restartBtn.onClick.RemoveAllListeners();
        }

        private void OnShopOpenedClickHandler()
        {
            OnShopOpened?.Invoke();
        }
        
        private void OnRestartClickHandler()
        {
            OnGameRestarted?.Invoke();
        }
    }
}
