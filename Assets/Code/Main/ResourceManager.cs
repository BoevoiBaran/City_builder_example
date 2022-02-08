using System.Collections.Generic;
using Code.Main.Data;
using UnityEngine;

namespace Code.Main
{
    public class ResourceManager
    {
        private readonly Dictionary<int, ResourceHolder> _resourceStorage = new Dictionary<int, ResourceHolder>(); 
        
        public ResourceManager(ResourceData resourceData)
        {
            InitializeStorage(resourceData);
        }

        private void InitializeStorage(ResourceData resourceData)
        {
            foreach (var (type, count, limit) in resourceData.Data)
            {
                _resourceStorage.Add((int) type, new ResourceHolder
                {
                    Limit = limit,
                    Value = count
                });    
            }
        }

        public ResourceData GetResourceInfo()
        {
            var result = new ResourceData
            {
                Data = new List<(ResourceType type, int count, int limit)>()
            };

            foreach (var kvp in _resourceStorage)
            {
                var type = (ResourceType) kvp.Key;
                var holder = kvp.Value;
                
                result.Data.Add((type, holder.Value, holder.Limit));
            }

            return result;
        }
        
        public void IncreaseResourceLimit(ResourceType type, int count)
        {
            if (count < 0)
            {
                return;    
            }
            
            if (_resourceStorage.TryGetValue((int) type, out var holder))
            {
                holder.Limit += count;
            }
        }

        public bool CanCollectResource(ResourceType type, int count)
        {
            if (count < 0)
            {
                return false;    
            }
            
            if (_resourceStorage.TryGetValue((int) type, out var holder))
            {
                var updatedValue = holder.Value + count;
                
                if (holder.Limit >= updatedValue)
                {
                    return true;
                }
            }
            
            return false;
        }

        public bool TryAddResource(ResourceType type, int count)
        {
            if (count < 0)
            {
                Debug.Log($"Incorrect resource count:{count} for add operation");
                return false;    
            }
            
            if (_resourceStorage.TryGetValue((int) type, out var holder))
            {
                var updatedValue = holder.Value + count;
                
                if (holder.Limit >= updatedValue)
                {
                    holder.Value = updatedValue;
                    return true;
                }
            }

            Debug.Log($"Incorrect resource type:{type} for add operation");
            
            return false;
        }

        public bool TrySpendResource(ResourceType type, int count)
        {
            if (count < 0)
            {
                Debug.Log($"Incorrect resource count:{count} for spend operation");
                return false;    
            }
            
            if (_resourceStorage.TryGetValue((int) type, out var holder))
            {
                if (holder.Value >= count)
                {
                    holder.Value -= count;
                    return true;
                }
            }

            Debug.Log($"Incorrect resource type:{type} for spend operation");
            return false;
        }
    }
}
