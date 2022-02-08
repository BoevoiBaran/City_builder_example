using System;
using Code.Extensions;
using UnityEngine;
using UnityEngine.UI;

namespace Code.UI
{
    public class ShopView : MonoBehaviour
    {
        public event Action<string> OnBuildingSelected;
        public event Action OnCloseClicked;
        
        [SerializeField] private Button gasStorageBtn;
        [SerializeField] private Button mineralStorageBtn;
        
        [SerializeField] private Button gasFarmBtn;
        [SerializeField] private Button mineralFarmBtn;
        
        [SerializeField] private Button closeBtn;
        
        public void Show()
        {
            gameObject.SetSafeActive(true);
        }

        public void Hide()
        {
            gameObject.SetSafeActive(false);
        }

        private void OnEnable()
        {
            gasStorageBtn.onClick.AddListener(OnGasStorageSelectHandler);
            mineralStorageBtn.onClick.AddListener(OnMineralStorageSelectHandler);
            gasFarmBtn.onClick.AddListener(OnGasFarmSelectHandler);
            mineralFarmBtn.onClick.AddListener(OnMineralFarmSelectHandler);
            closeBtn.onClick.AddListener(OnCloseClickHandler);
        }

        private void OnDisable()
        {
            gasStorageBtn.onClick.RemoveAllListeners();
            mineralStorageBtn.onClick.RemoveAllListeners();
            gasFarmBtn.onClick.RemoveAllListeners();
            mineralFarmBtn.onClick.RemoveAllListeners();
            closeBtn.onClick.RemoveAllListeners();
        }

        private void OnGasStorageSelectHandler()
        {
            OnBuildingSelected?.Invoke("building_gas_storage_1");
        }
        
        private void OnMineralStorageSelectHandler()
        {
            OnBuildingSelected?.Invoke("building_minerals_storage_1");
        }
        
        private void OnGasFarmSelectHandler()
        {
            OnBuildingSelected?.Invoke("building_gas_farm_1");
        }
        
        private void OnMineralFarmSelectHandler()
        {
            OnBuildingSelected?.Invoke("building_minerals_farm_1");
        }
        
        private void OnCloseClickHandler()
        {
            OnCloseClicked?.Invoke();
        }
    }
}
