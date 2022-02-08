using System.Collections.Generic;
using UnityEngine;

namespace Code.Main.Data
{
    [CreateAssetMenu(fileName = "Settings", menuName = "Data/Settings", order = 1)]
    public class SettingsStorage : ScriptableObject
    {
        private readonly Dictionary<string, BuildingData> _buildingData = new Dictionary<string, BuildingData>();
        
        public int GameFieldX => gameFieldX;
        public int GameFieldY => gameFieldY;

        [SerializeField] private int gameFieldX = 40;
        [SerializeField] private int gameFieldY = 40;
        
        [SerializeField] private int startGasCount = 150;
        [SerializeField] private int startMineralsCount = 150;
        
        [SerializeField] private int startGasLimit = 150;
        [SerializeField] private int startMineralsLimit = 150;

        [SerializeField] private List<FarmData> farmData;
        [SerializeField] private List<StorageData> storageData;

        public void InitializeSettings()
        {
            foreach (var data in farmData)
            {
                _buildingData.Add(data.DataId, data);
            }

            foreach (var data in storageData)
            {
                _buildingData.Add(data.DataId, data);
            }
        }

        public BuildingData GetBuildingData(string dataId)
        {
            if (_buildingData.TryGetValue(dataId, out var data))
            {
                return data;
            }

            Debug.LogError($"Incorrect building Id:{dataId}");
            
            return null;
        }
        
        public ResourceData GetStartResourceData()
        {
            return new ResourceData
            {
                Data = new List<(ResourceType type, int count, int limit)>
                {
                    (ResourceType.Gas, startGasCount, startGasLimit),
                    (ResourceType.Minerals, startMineralsCount, startMineralsLimit)
                }
            };
        }
    }
}
