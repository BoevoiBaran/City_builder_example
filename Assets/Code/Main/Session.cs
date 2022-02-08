using System;
using System.Collections.Generic;
using Code.Main.Data;
using Code.Session;
using Code.UI;
using UnityEngine;

namespace Code.Main
{
    public class Session : MonoBehaviour
    {
        private static int MaxRaycastHitsCount = 2;
        
        [SerializeField] private GameField gameField;
        [SerializeField] private Builder builder;

        private SettingsStorage _settingsStorage;
        private ResourceManager _resourceManager;
        private BuildingManager _buildingManager;
        private Hud _hudController;
        
        private readonly List<IUpdated> _updated = new List<IUpdated>();
        
        private Camera _mainCamera;
        private string _selectedBuilding;
        
        private readonly RaycastHit[] _rHits = new RaycastHit[MaxRaycastHitsCount];

        public void InitializeSession(SessionContext sessionContext, SettingsStorage settingsStorage)
        {
            _mainCamera = Camera.main;
            _settingsStorage = settingsStorage;

            
            _resourceManager = new ResourceManager(sessionContext.ResourceData);
            _buildingManager = new BuildingManager(_settingsStorage, builder);
            _updated.Add(_buildingManager);
            
            gameField.Initialize(sessionContext.FieldSizeX, sessionContext.FieldSizeY);
            
            _hudController = Core.Instance.UiManager.GetWindow<Hud>();
            _hudController.SetResourceLabel(_resourceManager.GetResourceInfo());
            _hudController.ResetSelectedBuildingLevel();

            _buildingManager.OnResourceLimitChanged += IncreaseResourceLimit;
        }

        public void SetSelectedBuilding(string id)
        {
            _selectedBuilding = id;
            _hudController.SetSelectedBuildingLabel(id);
        }

        private void IncreaseResourceLimit(ResourceType type, int count)
        {
            _resourceManager.IncreaseResourceLimit(type, count);
            _hudController.SetResourceLabel(_resourceManager.GetResourceInfo());
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                var hitCount = Physics.RaycastNonAlloc(_mainCamera.ScreenPointToRay(Input.mousePosition), _rHits);
                if (hitCount <= 0)
                {
                    return;
                }
                
                for (var hitIndex = 0; hitIndex < hitCount; hitIndex++)
                {
                    var node = _rHits[hitIndex].transform.gameObject.GetComponent<Node>();
                    if (node != null)
                    {
                        Build(node);
                        continue;
                    }
                    
                    var farm = _rHits[hitIndex].transform.gameObject.GetComponent<Farm>();
                    if (farm != null)
                    {
                        CollectResource(farm);    
                    }
                    
                    break;
                }
            } 
            
            ProcessCustomUpdate(Time.deltaTime);
        }

        private void ProcessCustomUpdate(float deltaTime)
        {
            for (int i = 0; i < _updated.Count; i++)
            {
                _updated[i].Update(deltaTime);
            }
        }
        
        private void Build(Node selectedNode)
        {
            if (selectedNode == null)
            {
                return;
            }

            if (string.IsNullOrEmpty(_selectedBuilding))
            {
                return;   
            }
            
            var buildingData = _settingsStorage.GetBuildingData(_selectedBuilding);

            if (buildingData == null)
            {
                return;
            }
                
            if (_resourceManager.TrySpendResource(buildingData.ResourceForBuilding, buildingData.BuildingPrice))
            {
                var nodeTransform = selectedNode.transform;
                _buildingManager.ConstructBuilding(_selectedBuilding, nodeTransform, nodeTransform.position);
                _hudController.SetResourceLabel(_resourceManager.GetResourceInfo());
                ResetSelectedBuilding();   
            }
            
            ResetSelectedBuilding();
        }

        private void ResetSelectedBuilding()
        {
            _selectedBuilding = null;
            _hudController.ResetSelectedBuildingLevel();  
        }
        
        private void CollectResource(Farm farm)
        {
            var id = farm.InstanceId;
            var farmResourceType = _buildingManager.GetFarmProducedResource(id);

            if (_resourceManager.CanCollectResource(farmResourceType.resourceType, farmResourceType.count))
            {
                var collectedResource = _buildingManager.CollectFarmProducedResource(id);
                if (_resourceManager.TryAddResource(collectedResource.resourceType, collectedResource.count))
                {
                    _hudController.SetResourceLabel(_resourceManager.GetResourceInfo());
                }
            }
        }
    }
}
