namespace Code.Main.Data
{
    public abstract class BuildingInstance
    {
        public bool IsComplete => TimeToBuildingCompleteInSeconds <= 0;
        protected float TimeToBuildingCompleteInSeconds;

        public bool TryCompleteBuilding(float deltaTime)
        {
            TimeToBuildingCompleteInSeconds -= deltaTime;
            return TimeToBuildingCompleteInSeconds <= 0;
        }
    }


    public class StorageInstance : BuildingInstance
    {
        public readonly string InstanceId;
        public readonly StorageData Data;
        public readonly Storage View;
        
        public StorageInstance(string instanceId, StorageData data, Storage view)
        {
            InstanceId = instanceId;
            Data = data;
            View = view;
            TimeToBuildingCompleteInSeconds = data.BuildingTimeInSeconds;
            
            View.SetData(InstanceId, Data.BuildingColor);
        }
    }

    public class FarmInstance : BuildingInstance
    {
        public int ProducedResource => _producedResource;
        
        public readonly string InstanceId;
        public readonly FarmData Data;
        public readonly Farm View;
        
        private int _producedResource;
        private float _remainingProductionTimeInSeconds;

        public FarmInstance(string instanceId, FarmData data, Farm view)
        {
            InstanceId = instanceId;
            Data = data;
            View = view;
            TimeToBuildingCompleteInSeconds = data.BuildingTimeInSeconds;

            _producedResource = 0;
            _remainingProductionTimeInSeconds = Data.ProductionTimeInSeconds;
            
            View.SetData(InstanceId, Data.BuildingMiningColor, Data.BuildingCollectColor);
        }

        public bool CanCollect => _remainingProductionTimeInSeconds <= 0 && _producedResource > 0; 
        
        public bool TryMineResource(float deltaTime)
        {
            if (_remainingProductionTimeInSeconds <= 0)
            {
                return false;
            }

            _remainingProductionTimeInSeconds -= deltaTime;
            
            if (_remainingProductionTimeInSeconds <= 0)
            {
                _producedResource = Data.ProducedResourceAmount;
                return true;
            }
            
            return false;
        }

        public (bool success, ResourceType resourceType, int count) TryCollectResources()
        {
            if (CanCollect)
            {
                _remainingProductionTimeInSeconds = Data.ProductionTimeInSeconds;
                var producedValue = _producedResource;
                _producedResource = 0;
                
                return (true, Data.ResourceType, producedValue);
            }
            
            return (false, Data.ResourceType, 0);
        }
    }
}
