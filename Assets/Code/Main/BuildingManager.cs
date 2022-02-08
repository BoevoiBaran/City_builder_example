using System;
using System.Collections.Generic;
using Code.Main.Data;
using UnityEngine;

namespace Code.Main
{
    public class BuildingManager : IUpdated
    {
        public event Action<ResourceType, int> OnResourceLimitChanged;
        
        private readonly SettingsStorage _settingsStorage;
        private readonly Builder _builder;
        
        private readonly List<BuildingInstance> _buildingsUnderConstruction = new List<BuildingInstance>();
        private readonly List<FarmInstance> _farmBuildings = new List<FarmInstance>();
        private readonly Dictionary<string, FarmInstance> _farmBuildingsHolder = new Dictionary<string, FarmInstance>();
        private readonly List<StorageInstance> _storageBuildings = new List<StorageInstance>();


        public BuildingManager(SettingsStorage settingsStorage, Builder builder)
        {
            _settingsStorage = settingsStorage;
            _builder = builder;
        }
        
        public void ConstructBuilding(string id, Transform root, Vector3 position) 
        {
            var data = _settingsStorage.GetBuildingData(id);
            
            switch (data)
            {
                case FarmData farmData:
                    ConstructFarm(farmData, root, position);
                    break;
                case StorageData storageData:
                    ConstructStorage(storageData, root, position);
                    break;
            }
        }

        public (ResourceType resourceType, int count) GetFarmProducedResource(string id)
        {
            if (_farmBuildingsHolder.TryGetValue(id, out var instance))
            {
                if (instance.CanCollect)
                {
                    return (instance.Data.ResourceType, instance.ProducedResource);    
                }
            }

            return (ResourceType.Unknown, 0);
        }

        public (ResourceType resourceType, int count) CollectFarmProducedResource(string id)
        {
            if (_farmBuildingsHolder.TryGetValue(id, out var instance))
            {
                var collectResult = instance.TryCollectResources();

                if (collectResult.success)
                {
                    Debug.Log($"Collect {collectResult.count} {collectResult.resourceType}");
                    instance.View.SetMiningState();
                    return (collectResult.resourceType, collectResult.count);
                }   
            }
            
            return (ResourceType.Unknown, 0);
        }    
        
        private void ConstructFarm(FarmData data, Transform root, Vector3 position) 
        {
            _builder.Build(data.DataId, root, position, ConstructionCb);
            
            void ConstructionCb(GameObject go)
            {
                var view = go.GetComponent<Farm>();

                if (view != null)
                {
                    var instance = new FarmInstance(GetInstanceId(), data, view);
                    _buildingsUnderConstruction.Add(instance);
                }
            }
        }
        
        private void ConstructStorage(StorageData data, Transform root, Vector3 position) 
        {
            _builder.Build(data.DataId, root, position, ConstructionCb);
            
            void ConstructionCb(GameObject go)
            {
                var view = go.GetComponent<Storage>();

                if (view != null)
                {
                    var instance = new StorageInstance(GetInstanceId(), data, view);
                    _buildingsUnderConstruction.Add(instance);
                }
            }
        }

        private string GetInstanceId()
        {
            return Guid.NewGuid().ToString();
        }
        
        void IUpdated.Update(float deltaTime)
        {
            ProcessMining(deltaTime);
            ProcessBuildingConstructions(deltaTime);
        }
        
        private void ProcessBuildingConstructions(float deltaTime)
        {
            for (var i = 0; i < _buildingsUnderConstruction.Count; i++)
            {
                var building = _buildingsUnderConstruction[i];

                if (!building.TryCompleteBuilding(deltaTime))
                {
                    continue;
                }
                
                FinishBuildingConstructions(building);
                _buildingsUnderConstruction.Remove(building);
                i--;
            }
        }
        
        private void ProcessMining(float deltaTime)
        {
            for (var i = 0; i < _farmBuildings.Count; i++)
            {
                var farm = _farmBuildings[i];

                if (farm.TryMineResource(deltaTime))
                {
                    farm.View.SetCanCollectState();
                }
            }
        }

        private void FinishBuildingConstructions(BuildingInstance building)
        {
            switch (building)
            {
                case FarmInstance farmBuilding:
                    RegisterFarm(farmBuilding);
                    break;
                case StorageInstance storageBuilding:
                    RegisterStorage(storageBuilding);
                    break;
            }
        }

        private void RegisterFarm(FarmInstance farm)
        {
            farm.View.SetMiningState();
            
            _farmBuildings.Add(farm);
            _farmBuildingsHolder.Add(farm.InstanceId, farm);
        }
        
        private void RegisterStorage(StorageInstance storage)
        {
            storage.View.SetColor();
            
            var storageData = storage.Data;
            OnResourceLimitChanged?.Invoke(storageData.ResourceType, storageData.Limit);
            _storageBuildings.Add(storage);
        }
    }
}
