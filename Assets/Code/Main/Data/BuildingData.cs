using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Code.Main.Data
{
    [Serializable]
    public abstract class BuildingData
    {
        public string DataId => dataId;
        public ResourceType ResourceForBuilding => resourceForBuilding;
        public int BuildingPrice => buildingPrice;
        public float BuildingTimeInSeconds => buildingTimeInSeconds;

        [SerializeField] private string dataId;
        [SerializeField] private ResourceType resourceForBuilding;
        [SerializeField] private int buildingPrice;
        [SerializeField] private float buildingTimeInSeconds;
    }

    [Serializable]
    public class StorageData : BuildingData
    {
        public ResourceType ResourceType => resourceType;
        public int Limit => limit;

        public Color BuildingColor => buildingColor;
    
        [SerializeField] private ResourceType resourceType;
        [SerializeField] private int limit;
        [SerializeField] private Color buildingColor;
    }


    [Serializable]
    public class FarmData : BuildingData
    {
        public ResourceType ResourceType => resourceType;
        public int ProducedResourceAmount => producedResourceAmount;
        public float ProductionTimeInSeconds => productionTimeInSeconds;
        public Color BuildingCollectColor => buildingCollectColor;
        public Color BuildingMiningColor => buildingMiningColor;
    
        [SerializeField] private ResourceType resourceType;
        [SerializeField] private int producedResourceAmount;
        [SerializeField] private float productionTimeInSeconds;
        [SerializeField] private Color buildingMiningColor;
        [SerializeField] private Color buildingCollectColor;
    }
}
